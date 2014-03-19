using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoStrategy.GameFiles;
using MonoStrategy.GameFiles.Networking.MaintanenceNetworking;
using MonoStrategy.GameFiles.Procedural;
using MonoStrategy.GuiSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GameStates
{
    class GameLobby : IGameState
    {
        private SpriteFont font;

        private Gui gui;
        private TerrainGenerator terrainGenerator;
        private int currentSeed;

        public GameLobby()
        {
            currentSeed = GameEngine.GetInstance().Client.ServerSeed;
            terrainGenerator = new TerrainGenerator();
            terrainGenerator.Generate(currentSeed);

            font = GameEngine.GetInstance().ResourceManager.GetSpriteFont(@"Gui\guiFont");

            gui = new Gui();
           // gui.AddLabel(new Vector2(100, 100), "Players");
            if(GameSettings.IsServer)
            {
                gui.AddButton(new Vector2(1000 - 35, 600), "Start Game", StartGame);
                gui.AddButton(new Vector2(1000, 400), "New Map", Generate);
                gui.AddButton(new Vector2(828, 400), "Load Map", Generate);
            }
            else
                gui.AddButton(new Vector2(1000, 600), "Ready", SetReady);


            gui.AddButton(new Vector2(100, 600), "Exit Lobby", MainMenu);
        }

        private void Generate()
        {
            GameEngine.GetInstance().Client.SendMaintenanceRequest(new ChangeSeedRequest(System.DateTime.Now.Millisecond));
        }

        public void StartGame()
        {
            GameEngine.GetInstance().Client.SendMaintenanceRequest(new StartGameRequest());
        }

        public void SetReady()
        {

        }

        public void MainMenu()
        {
            GameEngine.GetInstance().Client.SendMaintenanceRequest(new LeaveGameRequest(GameEngine.GetInstance().Client.MyIP));
            GameEngine.GetInstance().Client.Dissconnect();

            if (GameSettings.IsServer)
                GameEngine.GetInstance().Server.Shutdown();

            GameEngine.GetInstance().CurrentGameState = new MainMenu();
        }


        public void Update(float elapsedTime)
        {
            if(GameEngine.GetInstance().Client.ServerDisconnected)
            {
                GameEngine.GetInstance().CurrentGameState = new MainMenu();
                GameEngine.GetInstance().Client.Dissconnect();
            }
            else if(GameEngine.GetInstance().Client.StartGame)
            {
                GameEngine.GetInstance().CurrentGameState = new PlayingState(); ;
            }

            if (GameSettings.IsServer)
                GameEngine.GetInstance().Server.HandleMessages();

            GameEngine.GetInstance().Client.HandleMessages();

            if (currentSeed != GameEngine.GetInstance().Client.ServerSeed)
                terrainGenerator.Generate(currentSeed = GameEngine.GetInstance().Client.ServerSeed);



            gui.Update(elapsedTime);

        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spritebatch)
        {
            Texture2D border = GameEngine.GetInstance().ResourceManager.GetTexture("p");


            spritebatch.Begin();
            spritebatch.Draw(border, new Vector2(95, 50), null, Color.SaddleBrown * 0.5f, 0.0f, Vector2.Zero, new Vector2(200, 400), SpriteEffects.None, 0.0f);
       
            foreach (KeyValuePair<int, String> kvp in GameEngine.GetInstance().Client.ConnectedClients)
                spritebatch.DrawString(font,  kvp.Value.ToString(), new Vector2(110, 50*kvp.Key), Color.White);
            spritebatch.End();

            spritebatch.Begin();
            terrainGenerator.Draw(spritebatch, new Vector2(983, 200));
            spritebatch.End();

            gui.Draw(spritebatch);
        }
    }
}
