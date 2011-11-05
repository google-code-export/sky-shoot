using Microsoft.Xna.Framework;
using SkyShoot.Game.Client.View;

namespace SkyShoot.Game
{
    public class SkyShootGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private ScreenManager.ScreenManager _screenManager;

        public SkyShootGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Textures.GraphicsDevice = GraphicsDevice;

            _screenManager = new ScreenManager.ScreenManager(this);
            Components.Add(_screenManager);
            _screenManager.AddScreen(new Screens.BackgroundScreen(Color.LightSeaGreen));
            _screenManager.AddScreen(new Screens.MainMenuScreen());
            base.Initialize();

        }

        protected override void LoadContent()
        {

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

        }
    }
}
