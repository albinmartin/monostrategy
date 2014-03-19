using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoStrategy.GuiSystem
{
    public class GuiGraphic : GuiComponent
    {
        private Texture2D tex;

        public GuiGraphic(String texture, Vector2 position)
        {
            tex = GameEngine.GetInstance().ResourceManager.GetTexture(texture);
            this.Position = position;

            Bounds = new Vector2(tex.Width, tex.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, GetAbsolutePosition(), Color.White);
        }

        public override void Update(float elapsedTime)
        {
        }
    }
}
