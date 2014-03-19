using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace monostrategy.Utility
{
    public static class InputManager
    {
        private static KeyboardState lastKeyboardState;
        private static MouseState lastMouseState;

        private static GamePadState[] lastControllerState;
        private static Vector2 lastMouse = Vector2.Zero;
        private static int lastMouseScroll;

        public static void Initialize()
        {
            lastControllerState = new GamePadState[4];
        }

        public static void RefreshState()
        {
            lastKeyboardState = Keyboard.GetState();
            lastMouseState = Mouse.GetState();
            lastMouse = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            lastMouseScroll = Mouse.GetState().ScrollWheelValue;

            /*
            lastControllerState[0] = GamePad.GetState(PlayerIndex.One);
            lastControllerState[1] = GamePad.GetState(PlayerIndex.Two);
            lastControllerState[2] = GamePad.GetState(PlayerIndex.Three);
            lastControllerState[3] = GamePad.GetState(PlayerIndex.Four);
            */

        }

        public static GamePadState GetGamePadState(PlayerIndex playerIndex)
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

        public static bool IsLeftMouseUp()
        {
            return Mouse.GetState().LeftButton == ButtonState.Released;
        }

        public static bool IsRightMouseUp()
        {
            return Mouse.GetState().RightButton == ButtonState.Released;
        }

        public static bool IsLeftMouseDown()
        {
            return Mouse.GetState().LeftButton == ButtonState.Pressed;
        }

        public static bool IsRightMouseDown()
        {
            return Mouse.GetState().RightButton == ButtonState.Pressed;
        }

        public static bool IsLeftMousePressed()
        {
            return IsLeftMouseDown() && (lastMouseState.LeftButton == ButtonState.Released);
        }

        public static bool IsRightMousePressed()
        {
            return IsRightMouseDown() && (lastMouseState.RightButton == ButtonState.Released);
        }

        public static bool IsLeftMouseReleased()
        {
            return !IsLeftMouseDown() && (lastMouseState.LeftButton == ButtonState.Pressed);
        }

        public static bool IsRightMouseReleased()
        {
            return !IsRightMouseDown() && (lastMouseState.RightButton == ButtonState.Pressed);
        }

        public static bool ScrolledUp()
        {
            return lastMouseScroll - Mouse.GetState().ScrollWheelValue > 0;
        }

        public static bool ScrolledDown()
        {
            return lastMouseScroll - Mouse.GetState().ScrollWheelValue < 0;
        }

        public static Vector2 GetMousePosition()
        {
            return new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }

        public static Vector2 GetRelativeMouse()
        {
            return GetMousePosition() - lastMouse;
        }

        public static bool IsKeyPressed(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key) && !lastKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key) && lastKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyReleased(Keys key)
        {
            return !Keyboard.GetState().IsKeyDown(key) && lastKeyboardState.IsKeyDown(key);
        }

        public static bool IsButtonPressed(Buttons button, PlayerIndex playerIndex)
        {
            return GamePad.GetState(playerIndex).IsButtonDown(button) && !GetGamePadState(playerIndex).IsButtonDown(button);
        }

        public static bool IsButtonDown(Buttons button, PlayerIndex playerIndex)
        {
            return GamePad.GetState(playerIndex).IsButtonDown(button) && GetGamePadState(playerIndex).IsButtonDown(button);
        }

        public static bool IsButtonReleased(Buttons button, PlayerIndex playerIndex)
        {
            return !GamePad.GetState(playerIndex).IsButtonDown(button) && GetGamePadState(playerIndex).IsButtonDown(button);
        }

        public static bool IsLeftAnalogueLeft(PlayerIndex playerIndex)
        {
            return GamePad.GetState(playerIndex).ThumbSticks.Left.X <= -0.5f &&
                   GetGamePadState(playerIndex).ThumbSticks.Left.X > -0.5f;
        }

        public static bool IsLeftAnalogueRight(PlayerIndex playerIndex)
        {
            return GamePad.GetState(playerIndex).ThumbSticks.Left.X >= 0.5f &&
                   GetGamePadState(playerIndex).ThumbSticks.Left.X < 0.5f;
        }

        public static bool IsLeftTriggerPressed(PlayerIndex playerIndex)
        {
            return GamePad.GetState(playerIndex).Triggers.Left <= 0.5f &&
                   GetGamePadState(playerIndex).Triggers.Left > 0.5f;
        }

        public static bool IsRightTriggerPressed(PlayerIndex playerIndex)
        {
            return GamePad.GetState(playerIndex).Triggers.Right <= 0.5f &&
                   GetGamePadState(playerIndex).Triggers.Right > 0.5f;
        }
    }
}
