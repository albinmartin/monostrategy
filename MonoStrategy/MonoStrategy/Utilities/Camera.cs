using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoStrategy.Utility
{
    public class Camera
    {
        private Vector3 position = Vector3.Zero;
        private float pitch = 0.0f, yaw = 0.0f, roll = 0.0f;
        private float cameraDistance = 0.0f;
        private Matrix perspective;

        #region Getters & Setters
        public Matrix Perspective
        {
            get { return perspective; }
            set { perspective = value; }
        }

        public float CameraDistance
        {
            get { return cameraDistance; }
            set { cameraDistance = value; }
        }

        public float Roll
        {
            get { return roll; }
            set { roll = value; }
        }

        public float Yaw
        {
            get { return yaw; }
            set { yaw = value; }
        }

        public float Pitch
        {
            get { return pitch; }
            set { pitch = value; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        #endregion

        public Camera()
        {
            perspective = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GameEngine.GetInstance().GraphicsDevice.Viewport.AspectRatio, 1.0f, 30000.0f);
        }

        public void DoYaw(float rad)
        {
            yaw += rad;
        }

        public void DoPitch(float rad)
        {
            pitch += rad;
            
            if (pitch + 0.01f > Math.PI / 2)
                pitch = (float)Math.PI / 2 - 0.01f;
            else if (pitch - 0.01f < -Math.PI / 2)
                pitch = -(float)Math.PI / 2 + 0.01f;
        }

        public void MoveForward(float dist)
        {
            Vector3 tmp = Vector3.Transform(Vector3.Forward, Matrix.CreateFromYawPitchRoll(yaw, pitch, roll));
            position += tmp*dist;
        }

        public void StrafeRight(float dist)
        {
            Vector3 tmp = Vector3.Transform(Vector3.Right, Matrix.CreateFromYawPitchRoll(yaw, pitch, roll));
            position += tmp*dist;
        }

        public void MoveBackward(float dist)
        {
            MoveForward(-dist);
        }

        public void StrafeLeft(float dist)
        {
            StrafeRight(-dist);
        }

        public void MoveUp(float dist)
        {
            position += this.GetUp() * dist;
        }

        public void MoveDown(float dist)
        {
            position -= this.GetUp() * dist;
        }

        public Vector3 GetForward()
        {
            return Vector3.Transform(Vector3.Forward, Matrix.CreateFromYawPitchRoll(yaw, pitch, roll));
        }

        public Vector3 GetUp()
        {
            return Vector3.Transform(Vector3.Up, Matrix.CreateFromYawPitchRoll(yaw, pitch, roll));
        }

        public Matrix GetTransform()
        {
            Vector3 forward = Vector3.Transform(Vector3.Forward, Matrix.CreateFromYawPitchRoll(yaw, pitch, roll));
            Vector3 distancePos = position;
            distancePos.Z += cameraDistance;
            return Matrix.CreateLookAt(distancePos, position + forward, Vector3.Up);
        }
    }
}
