using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoStrategy.GuiSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GameFiles.Procedural
{
    class TerrainGenerator
    {
        private Gui gui;

        GuiLabel addLabel;
        GuiLabel subLabel;

        GuiLabel freq1Label;
        GuiLabel freq2Label;
        GuiLabel freq3Label;

        private float freq1 = 1.0f;
        private float freq2 = 3.0f;
        private float freq3 = 2.0f;

        private Texture2D heightMapTexture;
        private HeightMap heightMap;
        private int size = GameSettings.GridDimensionsX;
        internal HeightMap HeightMap
        {
            get { return heightMap; }
        }

        public TerrainGenerator()
        {
            gui = new Gui();
 
            addLabel = gui.AddLabel(new Vector2(100, 50), "Generator Add: " + GameSettings.GeneratorAdd.ToString());
            gui.AddSlider(new Vector2(100, 100), 300.0f, 1.0f, 100.0f, (float)GameSettings.GeneratorAdd, GeneratorAdd);
            subLabel = gui.AddLabel(new Vector2(100, 150), "Generator Sub: " + GameSettings.GeneratorSub.ToString());
            gui.AddSlider(new Vector2(100, 200), 300.0f, 0.05f, 20.0f, (float)GameSettings.GeneratorSub, GeneratorSub);

            freq1Label = gui.AddLabel(new Vector2(100, 250), "Freq1: " + freq1.ToString());
            gui.AddSlider(new Vector2(100, 300), 300.0f, 0.0f, 20.0f, freq1, Freq1);

            freq2Label = gui.AddLabel(new Vector2(100, 350), "Freq2: " + freq2.ToString());
            gui.AddSlider(new Vector2(100, 400), 300.0f, 0.0f, 20.0f, freq2, Freq2);

            freq3Label = gui.AddLabel(new Vector2(100, 450), "Freq3: " + freq3.ToString());
            gui.AddSlider(new Vector2(100, 500), 300.0f, 0.0f, 20.0f, freq3, Freq3);

        }

        public void Generate(int seed)
        {
            Random rand;
            rand = new Random(seed);


            heightMap = new HeightMap(size, seed);
            heightMap.AddPerlinNoise(freq1);
            heightMap.AddPerlinNoise(freq2);
            heightMap.AddPerlinNoise(freq3);
            heightMap.MakeIsland();
            heightMap.Perturb(32.0f, 32.0f);
            for (int i = 0; i < 10; i++)
                heightMap.Erode(16.0f);

            heightMap.Smoothen();

            heightMapTexture = new Texture2D(GameEngine.GetInstance().GraphicsDevice, size, size);
            Color[] texData = new Color[size * size];
            for(int x = 0; x < size; x++)
                for(int y = 0; y < size; y++)
                {
                    float height = Math.Max(heightMap.Heights[x, y], 0);
                  //  texData[y * size + x] = new Color(heightMap.Heights[x, y] + 0.1f, heightMap.Heights[x, y] + 0.1f, heightMap.Heights[x, y] + 0.1f);

                    if (height < 0.1f)
                        texData[y * size + x] = new Color(heightMap.Heights[x, y], heightMap.Heights[x, y], heightMap.Heights[x, y] + 1.0f);
                    else if (height < 0.2f)
                        texData[y * size + x] = new Color(heightMap.Heights[x, y] + 0.937f, heightMap.Heights[x, y] + 0.89f, heightMap.Heights[x, y] + 0.69f);
                    else if (height < 0.5f)
                        texData[y * size + x] = new Color(heightMap.Heights[x, y] + 0.0f, heightMap.Heights[x, y] + 1.0f, heightMap.Heights[x, y] + 0.0f);
                    else if (height < 0.85f)
                        texData[y * size + x] = new Color(heightMap.Heights[x, y] + 0.1f, heightMap.Heights[x, y] + 0.1f, heightMap.Heights[x, y] + 0.1f);
                    else
                        texData[y * size + x] = new Color(heightMap.Heights[x, y] + 0.7f, heightMap.Heights[x, y] + 0.7f, heightMap.Heights[x, y] + 0.7f);
                    
                }


            heightMapTexture.SetData<Color>(texData);

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 positon)
        {
            Texture2D border = GameEngine.GetInstance().ResourceManager.GetTexture("p");
            float scale = 1.5f;
            spriteBatch.Draw(border, positon - new Vector2(size, size) * (scale + 0.1f)/2, null, Color.SaddleBrown *0.5f, 0.0f, Vector2.Zero, new Vector2(size, size) * (scale + 0.1f), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(heightMapTexture, positon, null, Color.White, 0, (new Vector2(size, size) / 2), scale, SpriteEffects.None, 0.0f);
        }

        public void Update(float elapsedTime)
        {

        }

        private void GeneratorAdd(float val)
        {
            addLabel.Text = "Generator Add: " + ((int)val).ToString();
            GameSettings.GeneratorAdd = (int)val;
        }

        private void GeneratorSub(float val)
        {
            subLabel.Text = "Generator Sub: " + val.ToString();
            GameSettings.GeneratorSub = val;
        }

        private void Freq1(float val)
        {
            freq1Label.Text = "Freq1: " + val.ToString();
            freq1 = val;
        }
        private void Freq2(float val)
        {
            freq2Label.Text = "Freq2: " + val.ToString();
            freq2 = val;
        }
        private void Freq3(float val)
        {
            freq3Label.Text = "Freq3: " + val.ToString();
            freq3 = val;
        }



    }
}
