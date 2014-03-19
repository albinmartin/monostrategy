using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoStrategy.Utility;
using MonoStrategy.GameFiles.Utilities;
using MonoStrategy.GameFiles;

namespace MonoStrategy
{
    class MouseKeyboardStrategy
    {
        private Vector3 movement;
        private Vector3 lookat;
        private Camera observer;
        private InputManager input;
        private Vector2 prevMouse;
        private float speed;

        public MouseKeyboardStrategy(Camera observer, InputManager input)
        {
            this.observer = observer;
            this.input = input;
            speed = 1.0f;
        }

        float t = 0.0f;

        public void update(float elapsedTime)
        {
            Vector2 mouse = input.GetMousePosition();
            if (prevMouse == null)
                prevMouse = mouse;

            int centerX = (int)GameSettings.Resolution.X / 2;
            int centerY = (int)GameSettings.Resolution.Y / 2;

            Mouse.SetPosition(centerX, centerY);

            float mx = MathHelper.ToRadians((mouse.X - centerX) * 0.05f);
            float my = MathHelper.ToRadians((mouse.Y - centerY) * 0.05f);
            observer.DoYaw(-mx);
            observer.DoPitch(-my);

            lookat = observer.GetForward()*speed;
            movement = Vector3.Zero;
            if (input.IsKeyDown(Keys.W)){
               movement += new Vector3(lookat.X, 0, lookat.Z);
                observer.MoveForward(1.0f);
            }
            else if (input.IsKeyDown(Keys.S))
                movement -= new Vector3(lookat.X, 0, lookat.Z);
            if (input.IsKeyDown(Keys.A))
                movement += new Vector3(lookat.Z, 0, -lookat.X);
            else if (input.IsKeyDown(Keys.D))
                movement -= new Vector3(lookat.Z, 0, -lookat.X);
            if (input.IsKeyDown(Keys.Space))
            {
                movement += new Vector3(0, 1, 0);
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                movement += new Vector3(0, -1, 0);
            }

            if (movement != Vector3.Zero)
                movement.Normalize();

            observer.Position += movement;
            prevMouse = mouse; 
        }

        public Vector3 MovementVector
        {
            get { return movement; }
        }

        public Vector3 LookAtVector
        {
            get { return lookat; }
        }
    }
}
