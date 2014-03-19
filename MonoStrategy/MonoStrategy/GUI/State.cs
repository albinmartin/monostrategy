using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GuiSystem
{
    class State
    {
        public delegate bool Condition();
        public delegate void Action(float elapsedTime);

        class SwitchTo
        {
            public Condition condition;
            public State state;

            public SwitchTo(Condition condition, State state)
            {
                this.condition = condition;
                this.state = state;
            }
        }

        private List<SwitchTo> switches;
        private List<Action> actions;

        public State()
        {
            switches = new List<SwitchTo>();
            actions = new List<Action>();
        }

        public void AddSwitch(Condition condition, State state)
        {
            switches.Add(new SwitchTo(condition, state));
        }

        public void AddAction(Action action)
        {
            actions.Add(action);
        }

        public State GetNextState()
        {
            foreach (SwitchTo s in switches)
            {
                if (s.condition())
                    return s.state;
            }

            return this;
        }

        public void Think(float elapsedTime)
        {
            foreach (Action a in actions)
                a(elapsedTime);
        }
    }
}
