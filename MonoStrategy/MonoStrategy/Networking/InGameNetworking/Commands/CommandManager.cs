using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GameFiles.Networking.Commands
{
    public class CommandComparer : IComparer<Command>
    {
        int IComparer<Command>.Compare(Command x, Command y)
        {
            return x.Lockstep - y.Lockstep;
        }
    }

    public class CommandManager
    {
        private World world;
        private List<Command> commandBuffer;

        public CommandManager(World world)
        {
            this.world = world;
            commandBuffer = new List<Command>();
        }

        public void AddCommand(Command command)
        {
            commandBuffer.Add(command);
            commandBuffer.Sort(new CommandComparer());
            
        }

        public void Update(int currentLockstep)
        {
         
            while(commandBuffer.Count > 0 && commandBuffer.First().Lockstep <= currentLockstep)
            {
                commandBuffer[0].Execute(world);
                commandBuffer.RemoveAt(0);
            }
        }

        
    }
}
