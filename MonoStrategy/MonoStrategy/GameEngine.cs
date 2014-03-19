#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using MonoStrategy.GameFiles;
using MonoStrategy.GameFiles.Procedural;
using MonoStrategy.GameFiles.Utilities;
using MonoStrategy.GameFiles.SimpleRendering;
using MonoStrategy.GameFiles.Networking.Server;
using MonoStrategy.GameFiles.Networking.Client;
using MonoStrategy.GameFiles.Networking.Requests;
using MonoStrategy.Utility;
using MonoStrategy.GuiSystem;
using MonoStrategy.GameStates;
#endregion

namespace MonoStrategy
{
    public class GameEngine : Game
    {
        GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private ContentManager ContentManager;
        private InputManager inputManager;
        private ResourceManager resourceManager;
        private IGameState currentGameState;
        private Camera camera;
        private Client client;
        private Server server;
        private static GameEngine instance;
        private IGameState currentCurrentGameState;

        #region Getters & Setters
        internal IGameState CurrentGameState
        {
            get { return currentGameState; }
            set { currentGameState = value; }
        }

        public ResourceManager ResourceManager
        {
            get { return resourceManager; }
        }

        internal Server Server
        {
            get { return server; }
            set { server = value; }
        }

        internal Client Client
        {
            get { return client; }
            set { client = value; }
        }

        public static GameEngine GetInstance()
        {
            return instance;
        }
    
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }
        
        public InputManager InputManager
        {
            get { return inputManager; }
        }
        public Camera Camera
        {
            get { return camera; }
            set { camera = value; }
        }
        #endregion
        
        public GameEngine()
            : base()
        {
            instance = this;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = (int)GameSettings.Resolution.X;
            graphics.PreferredBackBufferHeight = (int)GameSettings.Resolution.Y;

            base.Content.RootDirectory = "Content";
            ContentManager = base.Content;
        }

        protected override void Initialize()
        {
            base.Initialize();

            resourceManager = new ResourceManager(base.Content, graphics.GraphicsDevice);
            inputManager = new InputManager();
            inputManager.Initialize();

            currentGameState = new MainMenu();
            camera = new Camera();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
   
        }

        protected override void UnloadContent()
        {
            client.Dissconnect();
            if(GameSettings.IsServer)
                server.Shutdown();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentCurrentGameState = currentGameState;
            inputManager.RefreshState();
            currentGameState.Update(0.016f);

            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.BurlyWood);

            //currentGameState.Draw(spriteBatch);
            currentCurrentGameState.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
