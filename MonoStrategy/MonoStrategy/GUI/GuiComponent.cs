using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoStrategy.GuiSystem
{
    public abstract class GuiComponent
    {
        private Vector2 position;

        public Vector2 Position
        {
            get { return GetAbsolutePosition(); }
            set { position = value; }
        }

        protected Vector2 bounds;

        public Vector2 Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        private GuiWindow parent = null;

        internal GuiWindow Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public GuiComponent()
        {
            
        }

        /*
        public void SetAbsolutePosition(Vector2 pos)
        {
            if (parent != null)
                position = parent.GetAbsolutePosition() - pos;
            else
                position = pos;
        }*/

        public Vector2 GetAbsolutePosition()
        {
            if (parent != null)
                return position + parent.GetAbsolutePosition();
            else
                return position;
        }

        public Vector2 GetParentPosition()
        {
            if (parent != null)
                return parent.GetAbsolutePosition();
            else
                return Vector2.Zero;
        }

        public virtual bool IsHovering()
        {
            Vector2 p = GameEngine.GetInstance().InputManager.GetMousePosition();
            Vector2 ap = GetAbsolutePosition();

            return p.X > ap.X && p.X < ap.X + bounds.X &&
                   p.Y > ap.Y && p.Y < ap.Y + bounds.Y;
        }

        public MonoStrategy.Utility.Rectangle Rectangle
        {
            get { return new MonoStrategy.Utility.Rectangle(Position, Bounds); } 
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Update(float elapsedTime);
    }
}
