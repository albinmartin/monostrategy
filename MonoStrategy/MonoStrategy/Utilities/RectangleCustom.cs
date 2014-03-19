using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoStrategy.Utility
{
    // (X,Y) marks the middle of the cube and not the top left corner as the regular Rectangle class
    public struct Rectangle
    {
        private float x;
        private float y;
        private float width;
        private float height;

        #region Getters & Setters
        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        public Vector2 Position
        {
            get { return new Vector2(x, y); }
            set
            {
                x = value.X;
                y = value.Y;
            }
        }

        public float Bottom
        {
            get { return y - height / 2; }
        }

        public float Right
        {
            get { return x + width / 2; }
        }

        public float Top
        {
            get { return y + height / 2; }
        }

        public float Left
        {
            get { return x - width / 2; }
        }

        public Vector2 TopLeft
        {
            get { return new Vector2(Left, Top); }
        }

        public Vector2 TopRight
        {
            get { return new Vector2(Right, Top); }
        }

        public Vector2 BottomRight
        {
            get { return new Vector2(Right, Bottom); }
        }

        public Vector2 BottomLeft
        {
            get { return new Vector2(Left, Top); }
        }

        public Vector2 Center
        {
            get { return new Vector2(Position.X + Bounds.X / 2, Position.Y + Bounds.Y / 2); }
        }

        public Vector2 Bounds
        {
            get { return new Vector2(width, height); }
            set
            {
                width = value.X;
                height = value.Y;
            }
        }
        #endregion

        public Rectangle(Vector2 position, Vector2 bounds)
        {
            x = position.X;
            y = position.Y;
            width = bounds.X;
            height = bounds.Y;
        }

        public Rectangle(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public bool Contains(Vector2 point)
        {
            return ((point.X > x - width/2.0) && (point.X < x + width/2.0) && (point.Y > y - height/2.0) && (point.Y < y + height/2.0));
        }

        public bool Contains(Rectangle rect)
        {
            return (rect.x >= x - width / 2.0) && (rect.x + rect.width <= x + width / 2.0) && (rect.y >= y - height / 2.0) && (rect.y + rect.height <= y + height / 2.0);
        }

        public bool Intersects(Rectangle rect)
        {
            return (this.Left > rect.Right) &&
                   (this.Right < rect.Left) &&
                   (this.Bottom > rect.Top) &&
                   (this.Top < rect.Bottom);
        }

        public override string ToString()
        {
            return Position.ToString();
        }
    }
}
