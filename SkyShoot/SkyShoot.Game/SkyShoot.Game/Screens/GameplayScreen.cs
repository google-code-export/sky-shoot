using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Client.Game;
using SkyShoot.Game.ScreenManager;
using Microsoft.Xna.Framework.Content;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SkyShoot.Game.Screens
{
    class GameplayScreen:GameScreen
    {
        ContentManager content;

        private GameModel _gameModel;

        public GameplayScreen()
        {
            //_gameModel = new GameModel();
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            //пока что делаем вид что загружаем контент xD
            Thread.Sleep(1000);

            ScreenManager.Game.ResetElapsedTime();
        }

        public override void UnloadContent()
        {
            if (content != null)
                content.Unload();
        }

        public override void HandleInput(InputState input)
        {            
            //SkyShoot.Client.Game.GameController.HandleInput(input);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            GraphicsDevice graphicsDevice = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            graphicsDevice.Clear(Color.SkyBlue);

            _gameModel.Draw(spriteBatch);
        }
    }
}
