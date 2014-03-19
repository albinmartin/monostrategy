using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace MonoStrategy.GameFiles.Utilities
{
    public class InputManager
    {
        private KeyboardState lastKeyboardState;
        private MouseState lastMouseState;

        private KeyboardState currentKeyboardState;
        private MouseState currentMouseState;


        private GamePadState[] lastControllerState;
        private Vector2 lastMouse = Vector2.Zero;
        private int lastMouseScroll;

        public void Initialize()
        {
            lastControllerState = new GamePadState[4];
        }

        public void RefreshState()
        {
            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            lastControllerState[0] = GamePad.GetState(PlayerIndex.One);
            lastControllerState[1] = GamePad.GetState(PlayerIndex.Two);
            lastControllerState[2] = GamePad.GetState(PlayerIndex.Three);
            lastControllerState[3] = GamePad.GetState(PlayerIndex.Four);



            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            lastMouse = new Vector2(lastMouseState.X, lastMouseState.Y);
            lastMouseScroll = lastMouseState.ScrollWheelValue;

        }

        public GamePadState GetGamePadState(PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.One)
                return lastControllerState[0];
            else if (playerIndex == PlayerIndex.Two)
                return lastControllerState[1];
            else if (playerIndex == PlayerIndex.Three)
                return lastControllerState[2];
            else
                return lastControllerState[3];
        }

        public bool IsLeftMouseUp()
        {
            return currentMouseState.LeftButton == ButtonState.Released;
        }

        public bool IsRightMouseUp()
        {
            return currentMouseState.RightButton == ButtonState.Released;
        }

        public bool IsLeftMouseDown()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool IsRightMouseDown()
        {
            return currentMouseState.RightButton == ButtonState.Pressed;
        }

        public  bool IsLeftMousePressed()
        {
            return IsLeftMouseDown() && (lastMouseState.LeftButton == ButtonState.Released);
        }

        public  bool IsRightMousePressed()
        {
            return IsRightMouseDown() && (lastMouseState.RightButton == ButtonState.Released);
        }

        public  bool IsLeftMouseReleased()
        {
            return !IsLeftMouseDown() && (lastMouseState.LeftButton == ButtonState.Pressed);
        }

        public  bool IsRightMouseReleased()
        {
            return !IsRightMouseDown() && (lastMouseState.RightButton == ButtonState.Pressed);
        }

        public  bool ScrolledUp()
        {
            return lastMouseScroll - currentMouseState.ScrollWheelValue > 0;
        }

        public  bool ScrolledDown()
        {
            return lastMouseScroll - currentMouseState.ScrollWheelValue < 0;
        }

        public  Vector2 GetMousePosition()
        {
            return new Vector2(currentMouseState.X, currentMouseState.Y);
        }

        public  Vector2 GetRelativeMouse()
        {
            return GetMousePosition() - lastMouse;
        }

        public  bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && !lastKeyboardState.IsKeyDown(key);
        }

        public  bool IsKeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyDown(key);
        }

        public  bool IsKeyReleased(Keys key)
        {
            return !currentKeyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyDown(key);
        }

        public  bool IsButtonPressed(Buttons button, PlayerIndex playerIndex)
        {
            return GamePad.GetState(playerIndex).IsButtonDown(button) && !GetGamePadState(playerIndex).IsButtonDown(button);
        }

        public  bool IsButtonDown(Buttons button, PlayerIndex playerIndex)
        {
            return GamePad.GetState(playerIndex).IsButtonDown(button) && GetGamePadState(playerIndex).IsButtonDown(button);
        }

        public  bool IsButtonReleased(Buttons button, PlayerIndex playerIndex)
        {
            return !GamePad.GetState(playerIndex).IsButtonDown(button) && GetGamePadState(playerIndex).IsButtonDown(button);
        }

        public  bool IsLeftAnalogueLeft(PlayerIndex playerIndex)
        {
            return GamePad.GetState(playerIndex).ThumbSticks.Left.X <= -0.5f &&
                   GetGamePadState(playerIndex).ThumbSticks.Left.X > -0.5f;
        }

        public  bool IsLeftAnalogueRight(PlayerIndex playerIndex)
        {
            return GamePad.GetState(playerIndex).ThumbSticks.Left.X >= 0.5f &&
                   GetGamePadState(playerIndex).ThumbSticks.Left.X < 0.5f;
        }

        public  bool IsLeftTriggerPressed(PlayerIndex playerIndex)
        {
            return GamePad.GetState(playerIndex).Triggers.Left <= 0.5f &&
                   GetGamePadState(playerIndex).Triggers.Left > 0.5f;
        }

        public  bool IsRightTriggerPressed(PlayerIndex playerIndex)
        {
            return GamePad.GetState(playerIndex).Triggers.Right <= 0.5f &&
                   GetGamePadState(playerIndex).Triggers.Right > 0.5f;
        }

        internal List<Keys> GetPressedKeys()
        {
            Keys[] lastPressed = lastKeyboardState.GetPressedKeys();
            Keys[] pressed = currentKeyboardState.GetPressedKeys();
            IEnumerable<Keys> both = pressed.Except(lastPressed);

            List<Keys> result = new List<Keys>();
            foreach (Keys key in both)
                result.Add(key);


            return result;
        }

        internal bool IsCharacter(Keys key)
        {
            return (key == Keys.A || key == Keys.B || key == Keys.C || key == Keys.D || key == Keys.E || key == Keys.F || key == Keys.G || key == Keys.H || key == Keys.I || key == Keys.J || key == Keys.K || key == Keys.L || key == Keys.M || key == Keys.N || key == Keys.O || key == Keys.P || key == Keys.Q || key == Keys.R || key == Keys.S || key == Keys.T || key == Keys.U || key == Keys.V || key == Keys.W || key == Keys.X || key == Keys.Y || key == Keys.Z);
 
        }

        internal bool IsDigit(Keys key)
        {
            return (key == Keys.D0 || key == Keys.D1 || key == Keys.D2 || key == Keys.D3 || key == Keys.D4 || key == Keys.D5 || key == Keys.D6 || key == Keys.D7 || key == Keys.D8 || key == Keys.D9);
  
        }
    }
}
