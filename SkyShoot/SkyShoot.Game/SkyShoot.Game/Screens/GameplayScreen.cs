using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Client.Game;
using SkyShoot.Client.View;

using SkyShoot.Game.ScreenManager;

namespace SkyShoot.Game.Screens
{
    class GameplayScreen : GameScreen
    {
        private ContentManager _content;

        private GameController _gameController;

        public GameplayScreen()
        {
            //todo uncomment this!
            //_gameController = new GameController();
        }

        public override void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            //load landscapes
            Textures.SandLandscape     = _content.Load<Texture2D>("Textures/Landscapes/SandLandscape");
            Textures.GrassLandscape    = _content.Load<Texture2D>("Textures/Landscapes/GrassLandscape");
            Textures.SnowLandscape     = _content.Load<Texture2D>("Textures/Landscapes/SnowLandscape");
            Textures.DesertLandscape   = _content.Load<Texture2D>("Textures/Landscapes/DesertLandscape");
            Textures.VolcanicLandscape = _content.Load<Texture2D>("Textures/Landscapes/VolcanicLandscape");

            //load stones
            for (int i = 1; i <= 4; i++)
                Textures.Stones[i - 1] = _content.Load<Texture2D>("Textures/Landscapes/Stone" + i);

            //todo temporary!
            _gameController = new GameController();

            ScreenManager.Game.ResetElapsedTime();
        }

        public override void UnloadContent()
        {
            if (_content != null)
                _content.Unload();
        }

        public override void HandleInput(InputState input)
        {            
            //SkyShoot.Client.Game.GameController.HandleInput(input);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphicsDevice = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            graphicsDevice.Clear(Color.SkyBlue);

            _gameController.GameModel.Draw(spriteBatch);
        }
    }
}
