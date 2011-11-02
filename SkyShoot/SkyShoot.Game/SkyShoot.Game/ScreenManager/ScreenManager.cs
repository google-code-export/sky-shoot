using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SkyShoot.Game.ScreenManager
{
    public class ScreenManager : DrawableGameComponent
    {
        List<GameScreen> screens = new List<GameScreen>();
        List<GameScreen> screensToUpdate = new List<GameScreen>();
        InputState input = new InputState();

        SpriteBatch spriteBatch;
        SpriteFont font;

        public SpriteBatch SpriteBatch { get { return spriteBatch; } }
        public SpriteFont Font { get { return font; } }

        public ScreenManager(Microsoft.Xna.Framework.Game game)
            : base(game)
        {

        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = content.Load<SpriteFont>("menufont");
            foreach (GameScreen screen in screens)
            {
                screen.LoadContent();
            }
        }
        protected override void UnloadContent()
        {
            foreach (GameScreen screen in screens)
            {
                screen.UnloadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            input.Update();

            screensToUpdate.Clear();
            foreach (GameScreen screen in screens)
                screensToUpdate.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            while (screensToUpdate.Count > 0)
            {
                GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];
                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(input);

                        otherScreenHasFocus = true;
                    }

                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }

        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screen.IsExiting = false;
            screen.LoadContent();
            screens.Add(screen);
        }

        public void RemoveScreen(GameScreen screen)
        {
            screen.UnloadContent();
            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }

        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }

    }
}
