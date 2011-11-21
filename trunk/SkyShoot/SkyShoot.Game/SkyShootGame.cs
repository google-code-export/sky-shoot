using Microsoft.Xna.Framework;
using SkyShoot.Game.Client.View;

namespace SkyShoot.Game
{
    public class SkyShootGame : Microsoft.Xna.Framework.Game
    {
        private ScreenManager.ScreenManager _screenManager;
        public SkyShootGame()
        {
            new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Textures.GraphicsDevice = GraphicsDevice;
            ScreenManager.ScreenManager.Init(this);
            _screenManager = ScreenManager.ScreenManager.Instance;
            Components.Add(_screenManager);

            base.Initialize();

            _screenManager.AddScreen(new Screens.BackgroundScreen(Color.LightSeaGreen));
            _screenManager.AddScreen(new Screens.MainMenuScreen());
        }

        protected override void LoadContent()
        {

        }

        protected override void UnloadContent()
        {

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);

        }
    }
}
