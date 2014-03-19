using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using System.Threading;
using MonoStrategy.GameFiles.Procedural;
using MonoStrategy.GameFiles.Networking.Requests;
using MonoStrategy.GameFiles.Networking.Commands;
using MonoStrategy.GameFiles.Networking.MaintanenceNetworking;
using System.Net;
using System.IO;

namespace MonoStrategy.GameFiles.Networking.Client
{
    public class Client
    {
        private NetPeerConfiguration config;
        private NetClient client;
        private Thread clientThread;
        private bool running;
        private World world;
        private int lockstep;
        private int lastCompletedLockstep = -1;
        private bool startGame;
        private int serverSeed = 0;

        public int ServerSeed
        {
            get { return serverSeed; }
        }

        public bool StartGame
        {
            get { return startGame; }
        }

        public bool ServerDisconnected
        {
            get { return !(client.ConnectionStatus == NetConnectionStatus.Connected); }
        }

        private Dictionary<int, String> connectedClients;

        public Dictionary<int, String> ConnectedClients
        {
            get { return connectedClients; }
            set { connectedClients = value; }
        }

        private int clientID = -1;
        private CommandManager commandManager;
        private String myIP;

        public int ClientID
        {
            get { return clientID; }
            set { clientID = value; }
        }

        public String MyIP
        {
            get { return myIP; }
        }


        public int Lockstep
        {
            get { return lockstep; }
            set { lockstep = value; }
        }


        public World World
        {
            get { return world; }
        }


        public Client()
        {
            config = new NetPeerConfiguration("Monostrategy"); // needs to be same on client and server!
            client = new NetClient(config);
            connectedClients = new Dictionary<int, string>();
        }
        
        public void SetWorld(World world)
        {
            this.world = world;
            commandManager = new CommandManager(world);
            
        }

        private string GetPublicIpAddress()
        {
            return GameSettings.PlayerName;
            var request = (HttpWebRequest)WebRequest.Create("http://ifconfig.me");

            request.UserAgent = "curl"; // this simulate curl linux command

            string publicIPAddress;

            request.Method = "GET";
            using (WebResponse response = request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    publicIPAddress = reader.ReadToEnd();
                }
            }

            return publicIPAddress.Replace("\n", "");
        }

        public void Update(float elapsedTime)
        {
            HandleMessages();
            if(world != null && clientID != -1)
            { 
                int currentLockstep = Lockstep;
                commandManager.Update(lockstep);
                if (currentLockstep > lastCompletedLockstep)
                {
                    lastCompletedLockstep = currentLockstep;
                    SendMaintenanceRequest(new IncreaseLockstepRequest(currentLockstep));
                }
            }
        }

        private void HandleMaintenanceCommands(String[] message, String plainText)
        {
            MaintenanceCommandTypes messageType = MaintenanceCommandTypes.Chat;
            int clientID = -1;
            String[] data = { "" };
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
                        break;
                    case MaintenanceCommandTypes.JoinGame:
                        if (data[0] == MyIP && this.clientID == -1)
                        {
                            this.clientID = clientID;
                            Console.WriteLine("CLIENT: You connected to server as Player" + clientID.ToString());
                        }
                        else if (!connectedClients.ContainsKey(clientID) || (connectedClients[clientID] != data[0]))
                        {
                            Console.WriteLine("CLIENT: Player" + clientID.ToString() + " connected to the server. (" + data[0] + ")");
                        }
                        if (!connectedClients.ContainsKey(clientID))
                            connectedClients.Add(clientID, data[0]);
                        else if ((connectedClients[clientID] != data[0]))
                            connectedClients[clientID] = data[0];

                        break;
                    case MaintenanceCommandTypes.Lockstep:
                        //Lockstep++;
                        Lockstep = int.Parse(data[0]);
                        break;
                    case MaintenanceCommandTypes.LeaveGame:
                        connectedClients.Remove(clientID);
                        Console.WriteLine("CLIENT: Player" + clientID.ToString() + " disconnected from the server. (" + data[0] + ")");
                        break;
                    case MaintenanceCommandTypes.StartGame:
                        Console.WriteLine("CLIENT: Starting game.");
                        startGame = true;
                        break;
                    case MaintenanceCommandTypes.ChangeSeed:
                        Console.WriteLine("CLIENT: Seed changed to:" + (int.Parse(data[0])).ToString());
                        serverSeed = int.Parse(data[0]);
                        break;
                }
            }
        }

        private void HandleInGameCommands(String[] message, String plainText)
        {
            //In game
            GameCommandTypes messageType = GameCommandTypes.Chat;
            int clientID = -1;
            int lockstep = -1;
            String[] data = { "-1" };

            try
            {
                messageType = (GameCommandTypes)Convert.ToInt32(message[1]);
                clientID = Convert.ToInt32(message[2]);
                lockstep = Convert.ToInt32(message[3]);
                data = message[4].Split(' ');
            }
            catch
            {
                Console.WriteLine("CLIENT: Network error 3");
            }
            finally
            {
                switch (messageType)
                {
                    case GameCommandTypes.Chat:
                        Console.WriteLine(message[4]);
                        break;
                    case GameCommandTypes.SpawnAgent:
                        commandManager.AddCommand(new SpawnAgentCommand(lockstep, int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3])));
                        world.AgentCount++;
                        break;
                }
            }
 
        }

        public  void HandleMessages()
        {
            NetIncomingMessage msg;
            while ((msg = client.ReadMessage()) != null)
            {
                NetIncomingMessageType type = msg.MessageType;
                switch (type)
                {
                    case NetIncomingMessageType.DebugMessage:
                        Console.WriteLine(msg.ReadString());
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        Console.WriteLine("CLIENT: New status: " + client.Status + " (Reason: " + msg.ReadString() + ")");
                        break;

                    case NetIncomingMessageType.Data:
                        // Handle data in buffer here
                        String plainText = msg.ReadString();
                        if (GameSettings.DEBUG)
                            Console.WriteLine("CLIENT: Received: " + plainText);
                        String[] message = plainText.Split(':');
                        MessageTypes messageType = MessageTypes.Default;

                        try
                        {
                            messageType = (MessageTypes)Convert.ToInt32(message[0]);
                        }
                        catch
                        {
                            Console.WriteLine("CLIENT: Network error 2");
                        }
                        finally
                        {
                            switch (messageType)
                            {
                                case MessageTypes.Maintenance:
                                    HandleMaintenanceCommands(message, plainText);
                                    break;
                                case MessageTypes.InGame:
                                    HandleInGameCommands(message, plainText);
                                    break;
                                default:
                                    Console.WriteLine("CLIENT: Network error 1");
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

        public void SendInGameString(String s)
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(((int)MessageTypes.InGame).ToString() + ":" + ((int)GameCommandTypes.Chat).ToString() + ":" + clientID.ToString() + ":" + (lockstep + GameSettings.MessageStepDelay).ToString() + ":" + s);
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendMaintenanceString(String s)
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(((int)MessageTypes.Maintenance).ToString() + ":" + ((int)MaintenanceCommandTypes.Chat).ToString() + ":" + clientID.ToString() + ":" + s);
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendMaintenanceRequest(MaintenanceRequest request)
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(request.GetMessage());
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendInGameRequest(Request request)
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(request.GetMessage());
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void Connect()
        {
            startGame = false;
            this.myIP = GetPublicIpAddress();
            client.Start();
            if(GameSettings.IsServer)
            {
                client.Connect("127.0.0.1", GameSettings.Port);
                while (client.GetConnection(new System.Net.IPEndPoint(System.Net.IPAddress.Parse("127.0.0.1"), GameSettings.Port)) == null) ;
            }
            else
            { 
                client.Connect(GameSettings.ConnectIP, GameSettings.Port);
                while (client.GetConnection(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(GameSettings.ConnectIP), GameSettings.Port)) == null) ;
            }
            //clientThread = new Thread(this.Run);
            //clientThread.Start();
            SendMaintenanceRequest(new JoinGameRequest(myIP));
           

        }

        public void Dissconnect()
        {
            startGame = false;
            if(running)
                clientThread.Abort();
            client.Disconnect("Bye bye server");
        }

    }
}
