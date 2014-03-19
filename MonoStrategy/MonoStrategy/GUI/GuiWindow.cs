using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoStrategy.GuiSystem
{
    public class GuiWindow : GuiComponent
    {
        private Texture2D p;
        private bool visible = false;

        private float vOffs = 0.0f;

        private List<GuiComponent> components;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public GuiWindow(Vector2 position, Vector2 bounds)
        {
            this.Position = position;
            this.Bounds = bounds;
            this.components = new List<GuiComponent>();

            p = GameEngine.GetInstance().ResourceManager.GetTexture("p");
        }

        public GuiComponent AddComponent(GuiComponent component)
        {
            components.Add(component);
            component.Parent = this;

            return component;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(p, Position, null, new Color(0.1f, 0.1f, 0.18f, 0.5f) * vOffs, 0.0f, Vector2.Zero, Bounds, SpriteEffects.None, 0.0f);

            spriteBatch.Draw(p, Position + new Vector2(0.0f, (Bounds.Y / 2.0f) * (1.0f - vOffs)), null, Color.White, 0.0f, Vector2.Zero, new Vector2(1.0f, (Bounds.Y / 2.0f) * vOffs), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(p, Position + new Vector2(0.0f, Bounds.Y / 2.0f), null, Color.White, 0.0f, Vector2.Zero, new Vector2(1.0f, (Bounds.Y / 2.0f) * vOffs), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(p, Position + new Vector2(Bounds.X, (Bounds.Y / 2.0f) * (1.0f - vOffs)), null, Color.White, 0.0f, Vector2.Zero, new Vector2(1.0f, (Bounds.Y / 2.0f) * vOffs), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(p, Position + new Vector2(Bounds.X, Bounds.Y / 2.0f), null, Color.White, 0.0f, Vector2.Zero, new Vector2(1.0f, (Bounds.Y / 2.0f) * vOffs), SpriteEffects.None, 0.0f);

            spriteBatch.Draw(p, Position + new Vector2((Bounds.X / 2.0f) * (1.0f - vOffs), 0.0f), null, Color.White, 0.0f, Vector2.Zero, new Vector2((Bounds.X / 2.0f) * vOffs, 1.0f), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(p, Position + new Vector2((Bounds.X / 2.0f) * (1.0f - vOffs), Bounds.Y), null, Color.White, 0.0f, Vector2.Zero, new Vector2((Bounds.X / 2.0f) * vOffs, 1.0f), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(p, Position + new Vector2(Bounds.X / 2.0f, 0.0f), null, Color.White, 0.0f, Vector2.Zero, new Vector2((Bounds.X / 2.0f) * vOffs, 1.0f), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(p, Position + new Vector2(Bounds.X / 2.0f, Bounds.Y), null, Color.White, 0.0f, Vector2.Zero, new Vector2((Bounds.X / 2.0f) * vOffs, 1.0f), SpriteEffects.None, 0.0f);

            if (visible)
            {
                foreach (GuiComponent gc in components)
                    gc.Draw(spriteBatch);
            }
        }

        public override void Update(float elapsedTime)
        {
            if (Visible)
            {
                vOffs += (1.0f - vOffs) / 5.0f;

                foreach (GuiComponent gc in components)
                    gc.Update(elapsedTime);
            }
            else
                vOffs += (0.0f - vOffs) / 5.0f;
        }
    }
}
