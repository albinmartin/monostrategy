using MonoStrategy.GameFiles.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GameFiles.AgentFiles
{
    public class Agent
    {
        private int x;

        public int X
        {
            get { return x; }
            set { x = value; }
        }
        private int y;

        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        private int z;

        public int Z
        {
            get { return z; }
            set { z = value; }
        }
        private int id;

        public Agent(int x, int y, int z, int id)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.id = id;
        }
    }
}
