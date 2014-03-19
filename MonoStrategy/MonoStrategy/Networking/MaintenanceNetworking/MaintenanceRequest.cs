using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GameFiles.Networking.MaintanenceNetworking
{
    public abstract class MaintenanceRequest
    {
        private readonly int commandType;
        private readonly int clientID;

        protected MaintenanceRequest(MaintenanceCommandTypes commandType)
        {
            this.commandType = (int)commandType;
            clientID = GameEngine.GetInstance().Client.ClientID;
        }

        public String GetMessage()
        {
            return ((int)MessageTypes.Maintenance).ToString() + ":" + commandType.ToString() + ":" + clientID.ToString() + ":" + MessageData();
        }

        protected abstract String MessageData();
    }

    public class IncreaseLockstepRequest : MaintenanceRequest
    {
        private int currentLockstep;

        public IncreaseLockstepRequest(int currentLockstep)
            :base(MaintenanceCommandTypes.Lockstep)
        {
            this.currentLockstep = currentLockstep;
        }

        protected override string MessageData()
        {
            return currentLockstep.ToString();
        }
    }

    public class JoinGameRequest : MaintenanceRequest
    {
        private String myIP;

        public JoinGameRequest(String myIP)
            : base(MaintenanceCommandTypes.JoinGame)
        {
            this.myIP = myIP;
        }

        protected override string MessageData()
        {
            return myIP.ToString();
        }
    }

    public class LeaveGameRequest : MaintenanceRequest
    {
        private String myIP;

        public LeaveGameRequest(String myIP)
            : base(MaintenanceCommandTypes.LeaveGame)
        {
            this.myIP = myIP;
        }

        protected override string MessageData()
        {
            return myIP.ToString();
        }
    }


    public class StartGameRequest : MaintenanceRequest
    {
        public StartGameRequest()
            : base(MaintenanceCommandTypes.StartGame)
        {
            
        }

        protected override string MessageData()
        {
            return "";
        }
    }

    public class ChangeSeedRequest : MaintenanceRequest
    {
        private int seed;
        public ChangeSeedRequest(int seed)
            : base(MaintenanceCommandTypes.ChangeSeed)
        {
            this.seed = seed;
        }

        protected override string MessageData()
        {
            return seed.ToString();
        }
    }
}
