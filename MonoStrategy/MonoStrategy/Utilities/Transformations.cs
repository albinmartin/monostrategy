using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoStrategy.Utility
{
    public class Transformations
    {
        public Matrix World;
        public Matrix View;
        public Matrix Projection;
        public Vector3 CameraPos;

        public Transformations()
        {
            World = View = Projection = Matrix.Identity;
            CameraPos = Vector3.Zero;
        }

        public Transformations(Matrix w, Matrix v, Matrix p)
        {
            World = w;
            View = v;
            Projection = p;
            CameraPos = Vector3.Zero;
        }

        public void SetMatrices(Matrix w, Matrix v, Matrix p)
        {
            World = w;
            View = v;
            Projection = p;
        }
    }
}
