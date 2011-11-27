using Microsoft.Xna.Framework;
using SkyShoot.Game.Client.View;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using SkyShoot.Game.ScreenManager;
using SkyShoot.Game.Client.Game;

namespace SkyShoot.Game
{
    public class SkyShootGame : Microsoft.Xna.Framework.Game
    {
        private ScreenManager.ScreenManager _screenManager;
        SpriteBatch spriteBatch;
        GraphicsDeviceManager _graphics;

        public SkyShootGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";          
            bool fscreen = Settings.Default.FullScreenSelected;
            switch (fscreen)
            {
                case false:
                    {
                        _graphics.PreferredBackBufferWidth = 800;
                        _graphics.PreferredBackBufferHeight = 600;
                    }
                    break;
                case true: _graphics.IsFullScreen = true;
                    break;
            }
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            Textures.GraphicsDevice = GraphicsDevice;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ScreenManager.ScreenManager.Init(this);
            _screenManager = ScreenManager.ScreenManager.Instance;
            Components.Add(_screenManager);

            base.Initialize();

            _screenManager.AddScreen(new Screens.BackgroundScreen(Color.LightSeaGreen));
            _screenManager.AddScreen(new Screens.MainMenuScreen());
        }

        protected override void LoadContent()
        {
            Textures.Arrow = Content.Load<Texture2D>("Textures/Cursors/Arrow");
            Textures.Plus = Content.Load<Texture2D>("Textures/Cursors/Plus");
            Textures.Cross = Content.Load<Texture2D>("Textures/Cursors/Cross");
            Textures.Target = Content.Load<Texture2D>("Textures/Cursors/Target");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            spriteBatch.Draw(Textures.ActiveCursor, Textures.GetCursorPosition(Mouse.GetState().X, Mouse.GetState().Y), Color.White);
            spriteBatch.End();           
        }
    }
}
