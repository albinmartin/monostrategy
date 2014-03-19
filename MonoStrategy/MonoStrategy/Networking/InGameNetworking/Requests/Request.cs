using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GameFiles.Networking.Requests
{
    public abstract class Request
    {
        private readonly int lockstep;
        private readonly int clientID;
        private readonly int commandType;


        public int Lockstep
        {
            get { return lockstep; }
        }

        protected Request(GameCommandTypes commandType, int lockstep)
        {
            this.lockstep = lockstep;
            this.clientID = GameEngine.GetInstance().Client.ClientID;
            this.commandType = (int)commandType;
        }

        public String GetMessage()
        {
            return ((int)MessageTypes.InGame).ToString() + ":" + 
                commandType.ToString() + ":" + 
                clientID.ToString() + ":" + 
                (lockstep + GameSettings.MessageStepDelay).ToString() + ":" + 
                MessageData();
        }

        protected abstract String MessageData();
    }
}
