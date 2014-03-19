using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GameFiles.Networking.Requests
{
    class SpawnAgentRequest : Request
    {
        int px;
        int py;
        int pz;

        public SpawnAgentRequest(int lockstep, int px, int py, int pz)
            :base(GameCommandTypes.SpawnAgent, lockstep)
        {
            this.px = px;
            this.py = py;
            this.pz = pz;
        }

        protected override String MessageData()
        {
            return px.ToString() + " " + py.ToString() + " " + pz.ToString();
        }
    }
}
