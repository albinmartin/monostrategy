using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GameFiles.Networking.Commands
{
    public class SpawnAgentCommand : Command
    {
        private int x;
        private int y;
        private int z;
        private int agentId;


        public SpawnAgentCommand(int lockstep, int x, int y, int z, int agentId)
            :base(lockstep)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.agentId = agentId;

        }

        public override void Execute(World world)
        {
            //SPAWN THE FUCKING AGENT
            Console.WriteLine("Spawning agent at: " + x.ToString() + ";" + y.ToString() + ";" + z.ToString() + " (" + agentId.ToString() + ")");
            world.Agents.Add(new AgentFiles.Agent(x, y, z, agentId));
        }
    }
}
