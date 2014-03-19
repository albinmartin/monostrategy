using MonoStrategy.GameFiles.AgentFiles;
using MonoStrategy.GameFiles.TerrainFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GameFiles
{
    public class World
    {
        private List<Agent> agents;
        private TerrainManager terrain;
        private int agentCount;

        public int AgentCount
        {
            get { return agentCount; }
            set { agentCount = value; }
        }

        public List<Agent> Agents
        {
            get { return agents; }
            set { agents = value; }
        }

        public TerrainManager Terrain
        {
            get { return terrain; }
        }

        public World()
        {
            terrain = new TerrainManager();
            agents = new List<Agent>();
            agentCount = 0;
        }

        public void Update(float elapsedTime, int currentLockstep)
        {

        }

    }
}
