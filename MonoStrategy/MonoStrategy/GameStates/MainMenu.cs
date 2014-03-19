using Microsoft.Xna.Framework;
using MonoStrategy.GameFiles;
using MonoStrategy.GameFiles.Networking.Client;
using MonoStrategy.GameFiles.Networking.Server;
using MonoStrategy.GuiSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GameStates
{
    class MainMenu : IGameState
    {

        private Gui gui;
        private GuiTextBox ipBox;
        private GuiTextBox nameBox;


        public MainMenu()
        {
            gui = new Gui();
            nameBox = gui.AddTextBox(new Vector2(500, 100), 200, 15);
            ipBox = gui.AddTextBox(new Vector2(500, 200), 200, 15);

            nameBox.Buffer = GameSettings.PlayerName;
            ipBox.Buffer = GameSettings.ConnectIP;

            gui.AddButton(new Vector2(100, 100), "Create Game", CreateGame);
            gui.AddButton(new Vector2(100, 200), "Join Game", JoinGame);
            
        }

        private void CreateGame()
        {
            GameSettings.IsServer = true;
            GameEngine.GetInstance().Server = new Server();
            GameEngine.GetInstance().Server.Start();
            

            GameEngine.GetInstance().Client = new Client();
            GameEngine.GetInstance().Client.Connect();

            while (GameEngine.GetInstance().Client.ClientID == -1)
            {
                GameEngine.GetInstance().Server.HandleMessages();
                GameEngine.GetInstance().Client.HandleMessages();
            }
            GameEngine.GetInstance().CurrentGameState = new GameLobby();
        }


        private void JoinGame()
        {
            GameSettings.IsServer = false;
            GameEngine.GetInstance().Client = new Client();
            GameEngine.GetInstance().Client.Connect();
            

            while (GameEngine.GetInstance().Client.ClientID == -1)
                GameEngine.GetInstance().Client.HandleMessages();

            GameEngine.GetInstance().CurrentGameState = new GameLobby();
        }

        public void Update(float elapsedTime)
        {
            gui.Update(elapsedTime);

            GameSettings.ConnectIP = ipBox.Buffer;
            GameSettings.PlayerName = nameBox.Buffer;
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spritebatch)
        {
            gui.Draw(spritebatch);
        }
    }
}
