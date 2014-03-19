using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoStrategy.GameFiles.AgentFiles;
using MonoStrategy.GameFiles.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GameFiles.SimpleRendering
{

    public class Camera2d
    {
        private float _zoom; // Camera Zoom
        private Matrix _transform; // Matrix Transform
        private Vector2 _pos; // Camera Position
        private float _rotation; // Camera Rotation
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; _zoom = Math.Max(0.00001f, _zoom); } // Negative zoom will flip image
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            _pos += amount;
        }
        // Get set position
        public Vector2 Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }
        public Matrix get_transformation(GraphicsDevice graphicsDevice)
        {
            _transform =       // Thanks to o KB o for this solution
              Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(GameSettings.Resolution.X * 0.5f, GameSettings.Resolution.Y * 0.5f, 0));
            return _transform;
        }
        public Camera2d()
        {
            _zoom = 0.05f;
            _rotation = 0.0f;
            _pos = new Vector2(10000, 10000);//Vector2.Zero;
        }

    }

    class SimpleWorldRenderer
    {
        Camera2d cam = new Camera2d();

        private void MoveCamera()
        {
            float speed = 30.0f/cam.Zoom;

            InputManager inputManager = GameEngine.GetInstance().InputManager;
            if (inputManager.IsKeyDown(Keys.W))
                cam.Move(new Vector2(0, -1.0f * speed));
            else if (inputManager.IsKeyDown(Keys.S))
                cam.Move(new Vector2(0, 1.0f * speed));
            
            if (inputManager.IsKeyDown(Keys.A))
                cam.Move(new Vector2(-1.0f * speed, 0));
            else if (inputManager.IsKeyDown(Keys.D))
                cam.Move(new Vector2(1.0f * speed, 0));

            if (inputManager.ScrolledDown() || inputManager.IsKeyDown(Keys.OemPlus))
                cam.Zoom *= 1.1f;
            else if (inputManager.ScrolledUp() || inputManager.IsKeyDown(Keys.OemMinus))
                cam.Zoom *= 0.9f;

        }

        public void Draw(World world, SpriteBatch spriteBatch)
        {
            MoveCamera();

            Texture2D texture = GameEngine.GetInstance().Content.Load<Texture2D>("whitetile");
            spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        cam.get_transformation(GameEngine.GetInstance().GraphicsDevice));

            for (int i = 0; i < world.Agents.Count; i++)
            {
                Agent agent = world.Agents[i];
                spriteBatch.Draw(texture, new Vector2(agent.X, agent.Z) * GameSettings.GridTileSize,
                                null,
                                new Color(128 * GameEngine.GetInstance().Client.ClientID, 64, 64, 255),
                                0,
                                new Vector2(GameSettings.GridTileSize / 2), 1.0f, SpriteEffects.None, 0.0f);
            }

            for(int x = 0; x < GameSettings.GridDimensionsX; x++)
                for(int z = 0; z < GameSettings.GridDimensionsZ; z++)
                    for (int y = 0; y < GameSettings.GridDimensionsY; y++)
                    {
                        if (x < 0 || y < 0 || z < 0 || x >= GameSettings.GridDimensionsX || y >= GameSettings.GridDimensionsY || z >= GameSettings.GridDimensionsZ)
                            continue;

                        float height = world.Terrain.Grid.SelectCube(x, y, z);
                        Color color = new Color(height, height, height);
                       
                        if(height > 0)
                            spriteBatch.Draw(texture, new Vector2(x, z) * GameSettings.GridTileSize, 
                                null, 
                                color, 
                                0, 
                                new Vector2(GameSettings.GridTileSize / 2), 1.0f, SpriteEffects.None, 1.0f);

                        /*switch (world.Terrain.Grid.SelectCube(x, y, z))
                        {
                            case TerrainFiles.TerrainTypes.Grass:
                                color = new Color(0, 255, 0, 255);
                                break;
                            case TerrainFiles.TerrainTypes.Rock:
                                color = new Color(64, 64, 64, 255);
                                break;
                            case TerrainFiles.TerrainTypes.Water:
                                color = new Color(0, 0, 255, 255);
                                break;
                        }

                        if(world.Terrain.Grid.SelectCube(x, y, z) != TerrainFiles.TerrainTypes.Empty)
                            spriteBatch.Draw(texture, new Vector2(x, z) * GameSettings.GridTileSize, 
                                null, 
                                color, 
                                0, 
                                new Vector2(GameSettings.GridTileSize / 2), 1.0f, SpriteEffects.None, 1.0f);*/
                    }



            spriteBatch.End();
        }
    }
}