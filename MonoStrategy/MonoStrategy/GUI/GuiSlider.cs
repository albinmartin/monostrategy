using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoStrategy.GuiSystem
{
    public class GuiSlider : GuiComponent
    {
        public delegate void ValueChanged(float value);

        private float minVal;
        private float maxVal;
        private float currentValue;

        public float CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value; }
        }

        private State currentState;

        private float width;

        private Texture2D p;

        private ValueChanged func;

        public float GetValue(float pc)
        {
            return minVal + pc*(Math.Abs(minVal-maxVal));
        }

        public void UpdateValue(float pc)
        {
            func(GetValue(pc));
        }

        public GuiSlider(Vector2 position, float width, float minVal, float maxVal, float defaultVal, ValueChanged valueChanged)
        {
            this.minVal = minVal;
            this.maxVal = maxVal;
            this.Position = position;
            this.width = width;

            this.currentValue = defaultVal;

            this.Bounds = new Vector2(width, 20.0f);

            this.func = valueChanged;

            State idleState = new State();
            State manipulatingState = new State();

            idleState.AddSwitch(new State.Condition(IsInteracting), manipulatingState);
            
            manipulatingState.AddSwitch(new State.Condition(StoppedInteracting), idleState);
            manipulatingState.AddAction(new State.Action(Interacting));

            currentState = idleState;

            p = GameEngine.GetInstance().ResourceManager.GetTexture("p");
        }

        private bool IsInteracting()
        {
            return (IsHovering() && GameEngine.GetInstance().InputManager.IsLeftMousePressed());
        }

        private bool StoppedInteracting()
        {
            return GameEngine.GetInstance().InputManager.IsLeftMouseUp();
        }

        private void Interacting(float elapsedTime)
        {
            Vector2 m = GameEngine.GetInstance().InputManager.GetMousePosition();

            if (m.X < GetAbsolutePosition().X)
                currentValue = minVal;
            else if (m.X > GetAbsolutePosition().X + Bounds.X)
                currentValue = maxVal;
            else
            {
                float pc = (m.X - GetAbsolutePosition().X) / width;
                currentValue = pc * maxVal;
            }

            func(currentValue);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(p, GetAbsolutePosition() + new Vector2(0.0f, Bounds.Y / 2.0f), null, Color.White, 0.0f, Vector2.Zero, new Vector2(width, 2.0f), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(p, GetAbsolutePosition() + new Vector2((currentValue / maxVal) * width, 0.0f), null, Color.White, 0.0f, Vector2.Zero, new Vector2(8.0f, Bounds.Y), SpriteEffects.None, 0.0f);
        }

        public override void Update(float elapsedTime)
        {
            currentState = currentState.GetNextState();
            currentState.Think(elapsedTime);
        }
    }
}
