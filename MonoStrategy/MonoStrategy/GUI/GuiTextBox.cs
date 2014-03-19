using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoStrategy.GuiSystem
{
    public class GuiTextBox : GuiComponent
    {
        private SpriteFont font;
        private Texture2D p;
        private Vector2 markerPos;
        private Vector2 draggedFromPosition;

        private int maxChars;

        private int cursorPosition = 16;
        private int draggedFrom = 16;

        private String buffer = "192.168.230.234";

        public delegate void SelectedOther(GuiTextBox other);
        public static event SelectedOther selectedOther;
        private bool selected = false;

        public String Buffer
        {
            get { return buffer; }
            set { if(value.Length < maxChars) buffer = value; }
        }

        public void Deselect(GuiTextBox other)
        {
            if (this != other)
            {
                selected = false;
                selectingBox = false;
                draggedFromPosition = markerPos;
                draggedFrom = cursorPosition;
            }
        }

        public GuiTextBox(Vector2 position, float width, int maxChars)
        {
            this.maxChars = maxChars;

            font = GameEngine.GetInstance().ResourceManager.GetSpriteFont(@"Gui\guiFont_small");
            p = GameEngine.GetInstance().ResourceManager.GetTexture("p");

            selectedOther += new SelectedOther(Deselect);
            this.Position = position;
            Bounds = new Vector2(width, font.MeasureString("A").Y);
        }

        float blinkTime = 0.0f;
        public override void Draw(SpriteBatch spriteBatch)
        {
            /*
            if (draggedFrom - cursorPosition != 0)
            {
                Vector2 left = markerPos;
                if (draggedFromPosition.X < markerPos.X)
                    left = draggedFromPosition;

                spriteBatch.Draw(p, new Vector2(left.X, left.Y + 6.0f), null, Color.Gray, 0.0f, Vector2.Zero, new Vector2(Math.Abs(markerPos.X - draggedFromPosition.X), bounds.Y - 12.0f), SpriteEffects.None, 1.0f);
            }*/

            Vector2 ap = GetAbsolutePosition();

            spriteBatch.Draw(p, ap, null, new Color(0.1f, 0.1f, 0.18f, 0.5f), 0.0f, Vector2.Zero, Bounds, SpriteEffects.None, 0.0f);

            if (IsHovering())
            {
                Vector2 m = GameEngine.GetInstance().InputManager.GetMousePosition();

                int offset = 0;
                while (offset < buffer.Length && ap.X + font.MeasureString(buffer.Substring(0, offset)).X < m.X)
                    offset++;

                Vector2 mPos = new Vector2(ap.X + font.MeasureString(buffer.Substring(0, offset)).X);

                spriteBatch.Draw(p, new Vector2(mPos.X, ap.Y + 6.0f), null, Color.Gray, 0.0f, Vector2.Zero, new Vector2(1.0f, Bounds.Y - 12.0f), SpriteEffects.None, 0.0f);
            }

            if (selected)
            {
                spriteBatch.Draw(p, ap, null, Color.White, 0.0f, Vector2.Zero, new Vector2(Bounds.X, 1.0f), SpriteEffects.None, 0.0f);
                spriteBatch.Draw(p, ap + new Vector2(0.0f, Bounds.Y - 2.0f), null, Color.White, 0.0f, Vector2.Zero, new Vector2(Bounds.X, 1.0f), SpriteEffects.None, 0.0f);
                spriteBatch.Draw(p, ap, null, Color.White, 0.0f, Vector2.Zero, new Vector2(1.0f, Bounds.Y), SpriteEffects.None, 0.0f);
                spriteBatch.Draw(p, ap + new Vector2(Bounds.X - 2.0f, 0.0f), null, Color.White, 0.0f, Vector2.Zero, new Vector2(1.0f, Bounds.Y), SpriteEffects.None, 0.0f);
            }
            else
            {
                spriteBatch.Draw(p, ap, null, Color.Gray, 0.0f, Vector2.Zero, new Vector2(Bounds.X, 1.0f), SpriteEffects.None, 0.0f);
                spriteBatch.Draw(p, ap + new Vector2(0.0f, Bounds.Y - 1.0f), null, Color.Gray, 0.0f, Vector2.Zero, new Vector2(Bounds.X, 1.0f), SpriteEffects.None, 0.0f);
                spriteBatch.Draw(p, ap, null, Color.Gray, 0.0f, Vector2.Zero, new Vector2(1.0f, Bounds.Y), SpriteEffects.None, 0.0f);
                spriteBatch.Draw(p, ap + new Vector2(Bounds.X - 1.0f, 0.0f), null, Color.Gray, 0.0f, Vector2.Zero, new Vector2(1.0f, Bounds.Y), SpriteEffects.None, 0.0f);
            }

            if (Math.Sin(blinkTime * 8.0f) > 0 && selected)
                spriteBatch.Draw(p, new Vector2(markerPos.X, ap.Y + 6.0f), null, Color.White, 0.0f, Vector2.Zero, new Vector2(1.0f, Bounds.Y - 12.0f), SpriteEffects.None, 0.0f);

          //  if(selected)
                spriteBatch.DrawString(font, buffer, Position + new Vector2(0.0f, 2.0f), Color.White);
           // else
           //     spriteBatch.DrawString(font, buffer, Position + new Vector2(0.0f, 2.0f), Color.Gray);
        }

        public void SetCursorPosition(int index)
        {
            cursorPosition = index;
            markerPos.X = GetAbsolutePosition().X + font.MeasureString(buffer.Substring(0, index)).X;
            draggedFrom = cursorPosition;
        }

        public void EnterString(String str)
        {
            RemoveSelection();

            buffer = buffer.Insert(cursorPosition, str);
            SetCursorPosition(cursorPosition + 1);
        }

        public bool RemoveSelection()
        {
            return false;
            if (draggedFrom - cursorPosition != 0)
            {
                buffer = buffer.Remove(Math.Min(draggedFrom, cursorPosition), Math.Abs(draggedFrom - cursorPosition));
                SetCursorPosition(Math.Min(draggedFrom, cursorPosition));

                return true;
            }

            return false;
        }

        private bool selectingBox = false;
        public override void Update(float elapsedTime)
        {
            Vector2 ap = GetAbsolutePosition();

            blinkTime += elapsedTime;
            if (IsHovering())
            {
                if (GameEngine.GetInstance().InputManager.IsLeftMouseUp())
                    selectingBox = false;

                Vector2 m = GameEngine.GetInstance().InputManager.GetMousePosition();

                int offset = 0;
                while (offset < buffer.Length && Position.X + font.MeasureString(buffer.Substring(0, offset)).X < m.X)
                    offset++;

                if (GameEngine.GetInstance().InputManager.IsLeftMousePressed())
                {
                    selectingBox = true;

                    draggedFrom = offset;
                    draggedFromPosition = new Vector2(ap.X + font.MeasureString(buffer.Substring(0, offset)).X, ap.Y);

                    SetCursorPosition(offset);
                }
                else if (GameEngine.GetInstance().InputManager.IsLeftMouseDown())
                {
                    markerPos = new Vector2(ap.X + font.MeasureString(buffer.Substring(0, offset)).X, ap.Y);
                    selectedOther(this);
                    selected = true;
                    cursorPosition = offset;
                }
            }

            if (selected)
            {
                if (GameEngine.GetInstance().InputManager.IsKeyPressed(Keys.Back) && cursorPosition > 0 && buffer.Length > 0)
                {
                    if (!RemoveSelection())
                    {
                        buffer = buffer.Remove(cursorPosition - 1, 1);
                        SetCursorPosition(cursorPosition - 1);
                    }
                }

                if (GameEngine.GetInstance().InputManager.IsKeyPressed(Keys.Delete) && cursorPosition < buffer.Length && buffer.Length > 0)
                {
                    if(!RemoveSelection())
                        buffer = buffer.Remove(cursorPosition, 1);
                }

                if (GameEngine.GetInstance().InputManager.IsKeyPressed(Keys.Left) && cursorPosition > 0)
                    SetCursorPosition(cursorPosition - 1);

                if (GameEngine.GetInstance().InputManager.IsKeyPressed(Keys.Right) && cursorPosition < buffer.Length)
                    SetCursorPosition(cursorPosition + 1);


                List<Keys> pressedKeys = GameEngine.GetInstance().InputManager.GetPressedKeys();
                foreach (Keys key in pressedKeys)
                {
                    if (buffer.Length < maxChars)
                    {
                        int k = (int)key;
                        if (key == Keys.OemPeriod)
                        {
                            EnterString(".");
                        }
                        if (GameEngine.GetInstance().InputManager.IsDigit(key))
                        {
                            EnterString("" + key.ToString()[1]);
                        }
                        else if (GameEngine.GetInstance().InputManager.IsCharacter(key))
                        {
                            if (GameEngine.GetInstance().InputManager.IsKeyDown(Keys.LeftShift) || GameEngine.GetInstance().InputManager.IsKeyDown(Keys.LeftShift))
                                EnterString("" + (char)key);
                            else
                                EnterString("" + (char)(key + 32));
                        }
                    }
                }
            }
        }
    }
}
