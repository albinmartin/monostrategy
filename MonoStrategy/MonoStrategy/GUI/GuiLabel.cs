using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoStrategy.GuiSystem
{
    public class GuiLabel : GuiComponent
    {
        private SpriteFont font;

        private String text;

        public String Text
        {
            get { return text; }
            set { text = value; }
        }
        private Color color;

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public GuiLabel(String fontName, Vector2 position, Color color, String text)
        {
            this.Position = position;
            this.font = GameEngine.GetInstance().ResourceManager.GetSpriteFont(fontName);
            this.text = text;
            this.color = color;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, GetAbsolutePosition(), color);
        }

        public override void Update(float elapsedTime)
        {
        }
    }
}
