using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoStrategy.GuiSystem
{
    public class Gui
    {
        private SpriteFont font;

        private Texture2D cursor;

        private List<GuiComponent> components = new List<GuiComponent>();

        public Gui()
        {
            font = GameEngine.GetInstance().ResourceManager.GetSpriteFont(@"Gui\guiFont");
            cursor = GameEngine.GetInstance().ResourceManager.GetTexture(@"Gui\guicursor");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            foreach (GuiComponent gc in components)
                gc.Draw(spriteBatch);

            spriteBatch.Draw(cursor, GameEngine.GetInstance().InputManager.GetMousePosition(), Color.White);
            spriteBatch.End();
        }

        public GuiButton AddButton(Vector2 position, String text, GuiButton.ButtonPressed func)
        {
            GuiButton gb = new GuiButton(text, position, func);
            components.Add(gb);

            return gb;
        }

        public GuiGraphic AddGraphic(String texture, Vector2 position)
        {
            GuiGraphic gg = new GuiGraphic(texture, position);
            components.Add(gg);

            return gg;
        }

        public GuiLabel AddLabel(Vector2 position, String text)
        {
            GuiLabel gl = new GuiLabel(@"Gui\guiFont", position, Color.White, text);
            components.Add(gl);

            return gl;
        }

        public GuiWindow AddWindow(Vector2 position, Vector2 bounds)
        {
            GuiWindow gw = new GuiWindow(position, bounds);
            components.Add(gw);

            return gw;
        }

        public GuiSlider AddSlider(Vector2 position, float length, float minVal, float maxVal, float defaultVal, GuiSlider.ValueChanged valueChanged)
        {
            GuiSlider gs = new GuiSlider(position, length, minVal, maxVal, defaultVal, valueChanged);
            components.Add(gs);

            return gs;
        }

        public GuiTextBox AddTextBox(Vector2 position, float width, int maxChars)
        {
            GuiTextBox gtb = new GuiTextBox(position, width, maxChars);
            components.Add(gtb);

            return gtb;
        }

        public void SliderChanged(GuiSlider slider, float pc)
        {
            slider.UpdateValue(pc);
        }

        public void Update(float elapsedTime)
        {
            foreach (GuiComponent gc in components)
                gc.Update(elapsedTime);
        }

        public void AddComponent(GuiComponent component)
        {
            components.Add(component);
        }

        internal void RemoveComponent(GuiComponent component)
        {
            components.Remove(component);
        }
    }
}
