using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using monostrategy.Utility;

namespace monostrategy.VBOs
{
    class WorldQuad
    {
        protected Vector3 position;
        protected Vector3 orientation;
        protected float roll;
        protected float yaw;
        protected float pitch;
        protected float scale;

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector3 Orientation
        {
            get { return orientation; }
            set 
            { 
                orientation = value;
                this.yaw = (float)-Math.Atan2(orientation.X, -orientation.Z);
                this.pitch = (float)Math.Atan2(orientation.Y, Math.Sqrt(Math.Pow(orientation.X, 2) + Math.Pow(orientation.Z, 2)));
            }
        }

        public float Roll
        {
            get { return roll; }
            set { roll = value; }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public WorldQuad(Vector3 position)
        {
            this.position = position;
            this.orientation = new Vector3(0, 0, -1);
            this.roll = 0.0f;
            this.pitch = 0.0f;
            this.yaw = 0.0f;
            this.scale = 1.0f;
            this.yaw = (float)-Math.Atan2(orientation.X, -orientation.Z);
            this.pitch = (float)Math.Atan2(orientation.Y, Math.Sqrt(Math.Pow(orientation.X, 2) + Math.Pow(orientation.Z, 2)));
        }

        public Matrix GetTransform()
        {
            return Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) * Matrix.CreateTranslation(position);
            //return Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) * Matrix.CreateTranslation(position);
        }

        public virtual void Draw(Effect effect, Transformations transformations)
        {
            Transformations trans = new Transformations(Matrix.Multiply(GetTransform(), transformations.World),transformations.View,transformations.Projection);
            QuadVBO.Instance.Draw(effect, trans);
        }

        public virtual void Draw(Effect effect, Transformations transformations, Vector3 pos)
        {
            Transformations trans = new Transformations(Matrix.Multiply(transformations.World, Matrix.CreateScale(scale) * Matrix.CreateTranslation(pos)), 
                transformations.View, 
                transformations.Projection);
            QuadVBO.Instance.Draw(effect, trans);
        }

        public virtual void DrawAlpha(Effect effect, Transformations transformations)
        {
            Transformations trans = new Transformations(Matrix.Multiply(transformations.World, GetTransform()), transformations.View, transformations.Projection);
            QuadVBO.Instance.DrawWithAlphablend(effect, trans);
        }
    }
}
