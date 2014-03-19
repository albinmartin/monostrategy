using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoStrategy.GuiSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GameFiles.Procedural
{
    class WorldGenerator
    {
        private Gui gui;
        private Random rand;
        private int seed;
        private World world;

        GuiLabel addLabel;
        GuiLabel subLabel;

        GuiLabel freq1Label;
        GuiLabel freq2Label;
        GuiLabel freq3Label;

        private float freq1 = 1.0f;
        private float freq2 = 3.0f;
        private float freq3 = 2.0f;


        public WorldGenerator(World world)
        {
            gui = new Gui();
            seed = 0;
            rand = new Random(seed);
            this.world = world;
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

            gui.AddButton(new Vector2(100, 600), "Generate", Generate);
        }


        private void GeneratorAdd(float val)
        {
            addLabel.Text = "Generator Add: " + ((int)val).ToString();
            GameSettings.GeneratorAdd = (int)val;
            //  generator.Generate();
        }

        private void GeneratorSub(float val)
        {
            subLabel.Text = "Generator Sub: " + val.ToString();
            GameSettings.GeneratorSub = val;
            // generator.Generate();
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

        public void Update(float elapsedTime)
        {
            gui.Update(elapsedTime);

        }
        public void Draw(SpriteBatch spritebatch)
        {
            gui.Draw(spritebatch);
        }

        public void Generate()
        {
            seed = System.DateTime.Now.Millisecond;
            HeightMap h = new HeightMap(GameSettings.GridDimensionsX, seed);
            h.AddPerlinNoise(freq1);
            h.AddPerlinNoise(freq2);
            h.AddPerlinNoise(freq3);
            h.MakeIsland();
            h.Perturb(32.0f, 32.0f);
            for (int i = 0; i < 10; i++)
                h.Erode(16.0f);

            h.Smoothen();

            for(int x = 0; x < GameSettings.GridDimensionsX; x++)
                for(int z = 0; z < GameSettings.GridDimensionsZ; z++)
                    for (int y = 0; y < GameSettings.GridDimensionsY; y++)
                    {
                        float height = h.Heights[x, z];
 
                            world.Terrain.Grid.SetCube(x, y, z, height);
                        /*else if (height < 0.5f)
                            world.Terrain.Grid.SetCube(x, y, z, TerrainFiles.TerrainTypes.Grass);
                        else
                            world.Terrain.Grid.SetCube(x, y, z, TerrainFiles.TerrainTypes.Rock);
                        /*if(rand.NextDouble() > 0.3)
                            world.Terrain.Grid.SetCube(x, y, z, TerrainFiles.TerrainTypes.Grass);
                        else
                            world.Terrain.Grid.SetCube(x, y, z, TerrainFiles.TerrainTypes.Rock);*/
                    }
        }
    }
}
