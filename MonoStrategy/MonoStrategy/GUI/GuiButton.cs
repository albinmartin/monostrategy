using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoStrategy.Utility;

namespace MonoStrategy.GuiSystem
{
    public class GuiButton : GuiComponent
    {
        public delegate void ButtonPressed();

        private String label;
        private ButtonPressed func;
        private Texture2D box;
       // private SoundEffect hover;
        private SpriteFont font;

        private bool playHover = true;

        private float boxWidth = 0.0f;
        private float boxHeight = 0.0f;
        private float colorMult = 0.3f;
        private Color color = Color.SaddleBrown;


        public GuiButton(String label, Vector2 position, ButtonPressed func)
        {
            font = GameEngine.GetInstance().ResourceManager.GetSpriteFont(@"Gui\guiFont");

            this.label = label;
            this.func = func;
            this.Position = position;
            this.Bounds = font.MeasureString(label);
            boxWidth = Bounds.X + 10;
            boxHeight = bounds.Y + 10;
            box = GameEngine.GetInstance().ResourceManager.GetTexture("p");
        }

        public override void Update(float elapsedTime)
        {
            if (IsHovering())
            {
                if (GameEngine.GetInstance().InputManager.IsLeftMousePressed())
                    func();


               /* boxWidth += (Bounds.X - boxWidth) / 5.0f;
                boxHeight += (2.0f - boxHeight) / 5.0f;*/
                colorMult += (2.0f - colorMult) / 5.0f;

                //if (playHover)
                //     hover.Play();

                playHover = false;
            }
            else
            {
                /*boxWidth += (10.0f - boxWidth) / 5.0f;
                boxHeight += (10.0f - boxHeight) / 5.0f;*/
                colorMult += (0.5f - colorMult) / 5.0f;

                playHover = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(box, GetAbsolutePosition() + new Vector2(-5.0f, -5.0f), null, color * colorMult, 0.0f, Vector2.Zero, new Vector2(boxWidth, boxHeight), SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, label, Position, Color.White);
        }
    }
}
