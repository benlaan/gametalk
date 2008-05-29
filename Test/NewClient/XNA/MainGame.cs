using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Laan.Riskier.Client.TileMap
{
    public class TileMap : Microsoft.Xna.Framework.Game
    {
        private FrameRateComponent      _frameRate;
        private MapComponent            _map;

        private GraphicsDeviceManager   _graphics;
        private SpriteBatch             _spriteBatch;

        public TileMap()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 950;

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            Window.AllowUserResizing = true;

            _frameRate = new FrameRateComponent(this);
            _map = new MapComponent(this);

            this.Components.Add(_map);
            this.Components.Add(_frameRate);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            Window.Title = String.Format("FPS: {0}", _frameRate.FPS);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
