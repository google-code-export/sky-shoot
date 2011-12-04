using System;

using SkyShoot.Game.ScreenManager;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkyShoot.Game.Screens
{
    class LoadingScreen:GameScreen
    {
        readonly GameScreen[] _screensToLoad;
        readonly bool _showLoadingMessage;
        bool _otherScreensAreGone;

        public LoadingScreen(bool showLoadingMessage, params GameScreen[] screensToLoad)
        {
            _screensToLoad = screensToLoad;
            _showLoadingMessage = showLoadingMessage;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void Update(GameTime gameTime, bool otherHasFocus,
                                               bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherHasFocus, coveredByOtherScreen);

            if (_otherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                foreach (GameScreen screen in _screensToLoad)
                {
                    if (screen != null)
                    {
                        ScreenManager.AddScreen(screen);
                    }
                }

                ScreenManager.Game.ResetElapsedTime();
            }
        }
        public override void Draw(GameTime gameTime)
        {
            if ((ScreenState == ScreenState.Active) &&
                (ScreenManager.GetScreens().Length == 1))
            {
                _otherScreensAreGone = true;
            }

            if (_showLoadingMessage)
            {

                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                SpriteFont font = ScreenManager.Font;

                const string message = "Loading...";

                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                var viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 textSize = font.MeasureString(message);
                Vector2 textPosition = (viewportSize - textSize) / 2;

                Color color = Color.White * TransitionAlpha;

                spriteBatch.Begin();
                spriteBatch.DrawString(font, message, textPosition, color);
                spriteBatch.End();
            }
        }

    }
}
