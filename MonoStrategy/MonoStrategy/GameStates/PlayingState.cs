using Microsoft.Xna.Framework;
using MonoStrategy.GameFiles;
using MonoStrategy.GameFiles.Networking.Client;
using MonoStrategy.GameFiles.Procedural;
using MonoStrategy.GameFiles.SimpleRendering;
using MonoStrategy.GameFiles.TerrainFiles;
using MonoStrategy.GuiSystem;
using MonoStrategy.Utility;
using MonoStrategy.VoxelStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GameStates
{
    class PlayingState : IGameState
    {
        private Renderer worldRenderer;
        private Gui gui;
        private Client client;
        private World world;
        private VoxelManager voxelManager;
        private MouseKeyboardStrategy movement;

        public PlayingState()
        {
            gui = new Gui();
            world = new World();
            voxelManager = new VoxelManager();
            client = GameEngine.GetInstance().Client;
            client.SetWorld(world);
            worldRenderer = new Renderer(GameEngine.GetInstance().GraphicsDevice);

            movement = new MouseKeyboardStrategy(GameEngine.GetInstance().Camera, GameEngine.GetInstance().InputManager);
            GenerateWorld();
        }

        public void Update(float elapsedTime)
        {
            //Gui
            gui.Update(elapsedTime);

            //Network
            if (GameSettings.IsServer)
                GameEngine.GetInstance().Server.Update(elapsedTime);
            client.Update(elapsedTime);
            if (GameEngine.GetInstance().InputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.P))
            {
                client.SendInGameRequest(new MonoStrategy.GameFiles.Networking.Requests.SpawnAgentRequest(client.Lockstep, client.ClientID, client.ClientID, client.ClientID + client.World.AgentCount));
            }

            //World
            movement.update(0.016f);
            world.Update(elapsedTime, client.Lockstep);
            voxelManager.Update();
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spritebatch)
        {
            gui.Draw(spritebatch);
            if (voxelManager.VertexAdded)
            {
                worldRenderer.SetVertexBuffer(voxelManager.GetMesh());
            }
            worldRenderer.Draw();
        }

        public void GenerateWorld()
        {
            int seed = System.DateTime.Now.Millisecond;
            HeightMap h = new HeightMap(GameSettings.GridDimensionsX, GameEngine.GetInstance().Client.ServerSeed);
            h.AddPerlinNoise(1.0f);
            h.AddPerlinNoise(3.0f);
            h.AddPerlinNoise(2);
            h.MakeIsland();
            //h.Perturb(32.0f, 32.0f);
            for (int i = 0; i < 30; i++)
            {
                h.Erode(16.0f);
            }

            h.Smoothen();
            for (int x = 0; x < GameSettings.GridDimensionsX; x++)
                for (int y = 0; y < GameSettings.GridDimensionsY; y++)
                    for (int z = 0; z < GameSettings.GridDimensionsZ; z++)
                    {
                        float height = h.Heights[x, z];
                        float bheight = (float)y / 50.0f;//GameSettings.GridDimensionsY;
                        if (bheight < height)
                        {
                            if (height < 0.11f)
                                voxelManager.Add(new Vector3(x, y, z), TerrainTypes.Water);
                            else if (height < 0.12f)
                                voxelManager.Add(new Vector3(x, y, z), TerrainTypes.Sand);
                            else if (height < 0.25f)
                                voxelManager.Add(new Vector3(x, y, z), TerrainTypes.Grass);
                            else if (height < 0.55f)
                                voxelManager.Add(new Vector3(x, y, z), TerrainTypes.Rock);
                            else
                                voxelManager.Add(new Vector3(x, y, z), TerrainTypes.Snow);
                        }
                    }

            voxelManager.Update();
        }
    }
}
