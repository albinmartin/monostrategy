using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using System.Threading;
using MonoStrategy.GameFiles.Procedural;
using MonoStrategy.GameFiles.Networking.Commands;
using MonoStrategy.GameFiles.Networking.MaintanenceNetworking;


namespace MonoStrategy.GameFiles.Networking.Server
{
    class ClientData
    {
        public int ID;
        public String IP;
        public int LockStep;
    }

    class Server
    {
        private NetPeerConfiguration config;
        private NetServer server;
        private bool running;
        private Thread serverThread;
        private World world;
        private int lockstep;
        private CommandManager commandManager;
        private Dictionary<int, ClientData> connectedClients;
        private int serverSeed;

        public Server()
        {
            world = new World();
            WorldGenerator generator = new WorldGenerator(world);
            generator.Generate();


            config = new NetPeerConfiguration("Monostrategy"); // needs to be same on client and server!
            config.MaximumConnections = 32;
            config.Port = GameSettings.Port;
            
            lockstep = 0;


            connectedClients = new Dictionary<int, ClientData>();
            commandManager = new CommandManager(world);
        }

        public void Start()
        {
            server = new NetServer(config);
            server.Start();
            //serverThread = new Thread(this.Run);
            //serverThread.Start();
        }

        public void Shutdown()
        {
            if(running)
                serverThread.Abort();
            server.Shutdown("Bye bye clients");
        }

        public void SendInGameString(String s)
        {
            NetOutgoingMessage msg = server.CreateMessage();

            msg.Write(((int)MessageTypes.InGame).ToString() + ":" + ((int)GameCommandTypes.Chat).ToString() + ":" + "0" + ":" + lockstep.ToString() + ":"  + s);

            server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendMaintenanceString(String s)
        {
            NetOutgoingMessage msg = server.CreateMessage();

            msg.Write(((int)MessageTypes.Maintenance).ToString() + ":" + ((int)MaintenanceCommandTypes.Chat).ToString() + ":" + "0" + ":" + s);

            server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
        }


        private void ForwardMessage(String message)
        {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write(message);
            server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
        }

        private void HandleMaintenanceRequests(String[] message, String plainText)
        {
            MaintenanceCommandTypes messageType = MaintenanceCommandTypes.Chat;
            int clientID = -1;
            String[] data = {""};
            try
            {
                messageType = (MaintenanceCommandTypes)Convert.ToInt32(message[1]);
                clientID = Convert.ToInt32(message[2]);
                data = message[3].Split(' ');
            }
            catch
            {

            }
            finally
            {
                switch (messageType)
                {
                    case MaintenanceCommandTypes.Chat:
                        Console.WriteLine(message[3]);
                        ForwardMessage(plainText);
                        break;
                    case MaintenanceCommandTypes.JoinGame:
                        ClientData clientData = new ClientData();
                        clientID = connectedClients.Count + 1;
                        clientData.ID = clientID;
                        clientData.IP = data[0];
                        connectedClients.Add(clientID, clientData);
                        Console.WriteLine("SERVER: Player" + clientData.ID + " connected. (" + clientData.IP + ")");
                        //plainText = plainText.Replace("-1", clientID.ToString()); //Replace the client ID
                        //ForwardMessage(plainText);

                        //Tell all players about connection:
                        foreach (KeyValuePair<int, ClientData> kvp in connectedClients) //Resend all connections
                            ForwardMessage(((int)MessageTypes.Maintenance).ToString() + ":" + ((int)MaintenanceCommandTypes.JoinGame).ToString() + ":" + kvp.Key.ToString() + ":" + kvp.Value.IP);

                        //Tell all players the seed:
                        ForwardMessage(new ChangeSeedRequest(serverSeed).GetMessage());
 
                        break;
                    case MaintenanceCommandTypes.Lockstep:
                        connectedClients[clientID].LockStep = lockstep;
                        TryIncreaseLockstep();
                        break;
                    case MaintenanceCommandTypes.LeaveGame:
                        connectedClients.Remove(clientID);
                        Console.WriteLine("SERVER: Player" + clientID + " dissconnected. (" + data[0] + ")");
                        ForwardMessage(plainText);
                        break;
                    case MaintenanceCommandTypes.StartGame:
                        Console.WriteLine("SERVER: Starting game.");
                        ForwardMessage(plainText);
                        break;
                    case MaintenanceCommandTypes.ChangeSeed:
                        Console.WriteLine("SERVER: Seed changed to: " + (int.Parse(data[0])).ToString());
                        serverSeed = int.Parse(data[0]);
                        ForwardMessage(plainText);
                        break;

                    
                }
            }
        }

        public void TryIncreaseLockstep()
        {
            bool nextStep = true;
            foreach (KeyValuePair<int, ClientData> p in connectedClients)
            {
                if (p.Value.LockStep < lockstep)
                {
                    nextStep = false;
                    break;
                }
            }
            if (nextStep)
            {
                lockstep++;
                ForwardMessage(((int)MessageTypes.Maintenance).ToString() + ":" + ((int)MaintenanceCommandTypes.Lockstep) + ":" + "0" + ":" + lockstep);
                //Console.WriteLine("SERVER: Lockstep " + lockstep);
            }
        }

        private void HandleInGameRequests(String[] message, String plainText)
        {
            //In game
            GameCommandTypes messageType = GameCommandTypes.Chat;
            int clientID = -1;
            int lockstep = -1;
            
            String[] data = {"-1"};

            try
            {
                messageType = (GameCommandTypes)Convert.ToInt32(message[1]);
                clientID = Convert.ToInt32(message[2]);
                lockstep = Convert.ToInt32(message[3]);
                data = message[4].Split(' ');
            }
            catch
            {

            }
            finally
            {
                switch (messageType)
                {
                    case GameCommandTypes.Chat:
                        Console.WriteLine(message[4]);
                        ForwardMessage(plainText);
                        break;
                    case GameCommandTypes.SpawnAgent:
                        commandManager.AddCommand(new SpawnAgentCommand(lockstep, int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]), world.AgentCount));
                        plainText += " " + (world.AgentCount).ToString();
                        world.AgentCount++;
                        ForwardMessage(plainText);
                        break;
                }
            }
        }

        public void HandleMessages()
        {
            
            NetIncomingMessage msg;
            while ((msg = server.ReadMessage()) != null)
            {
                NetIncomingMessageType type = msg.MessageType;
                switch (type)
                {
                    case NetIncomingMessageType.DebugMessage:
                        Console.WriteLine(msg.ReadString());
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        Console.WriteLine("SERVER: New status: " + server.Status + " (Reason: " + msg.ReadString() + ")");
                        break;

                    case NetIncomingMessageType.Data:
                        // Handle data in buffer here
                        String plainText = msg.ReadString();
                        if (GameSettings.DEBUG)
                            Console.WriteLine("SERVER: Received: " + plainText);
                        String[] message = plainText.Split(':');
                        MessageTypes messageType = MessageTypes.Default;

                        try
                        {
                            messageType = (MessageTypes)Convert.ToInt32(message[0]);
                        }
                        catch
                        {

                        }
                        finally
                        {
                            switch (messageType)
                            {
                                case MessageTypes.Maintenance:
                                    HandleMaintenanceRequests(message, plainText);
                                    break;
                                case MessageTypes.InGame:
                                    HandleInGameRequests(message, plainText);
                                    break;
                                default:
                                    Console.WriteLine("SERVER: Network error 1");
                                    break;
                            }
                        }


                        break;
                }
            }
        }

        private void Run()
        {
            running = true;
            while (running)
            {
                HandleMessages();  
            }
        }


        internal void Update(float elapsedTime)
        {
            int currentLockstep = lockstep;
            world.Update(elapsedTime, currentLockstep);
            commandManager.Update(currentLockstep);

            HandleMessages();
        }
    }
}
