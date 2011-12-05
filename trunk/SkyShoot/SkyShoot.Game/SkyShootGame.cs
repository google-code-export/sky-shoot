using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Game.Client.View;

namespace SkyShoot.Game
{
    public class SkyShootGame : Microsoft.Xna.Framework.Game
    {
        private ScreenManager.ScreenManager _screenManager;
        private SpriteBatch _spriteBatch;
        private readonly GraphicsDeviceManager _graphics;

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
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ScreenManager.ScreenManager.Init(this);
            _screenManager = ScreenManager.ScreenManager.Instance;
            Components.Add(_screenManager);

            base.Initialize();

            _screenManager.AddScreen(new Screens.LoginScreen());
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
            _spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            float x, y;
            ScreenManager.ScreenManager.Instance.GetMouseState(out x, out y);
            _spriteBatch.Draw(Textures.ActiveCursor, Textures.GetCursorPosition(x, y), Color.White);
            _spriteBatch.End();           
        }
    }
}
