using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GameFiles.Networking.Commands
{
    public abstract class Command
    {
        private readonly int lockstep; //At what step should it be executed?


        public int Lockstep
        {
            get { return lockstep; }
        }

        protected Command(int lockstep)
        {
            this.lockstep = lockstep;
        }

        public abstract void Execute(World world);

    }
}
