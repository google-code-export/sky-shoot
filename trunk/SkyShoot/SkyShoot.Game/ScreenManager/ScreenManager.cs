using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Nuclex.Input;
using Nuclex.UserInterface;


namespace SkyShoot.Game.ScreenManager
{
    public class ScreenManager : DrawableGameComponent
    {
        GuiManager _gui;
        InputManager _inputManager;

        public GuiManager Gui { get { return _gui; } }

        readonly List<GameScreen> _screens = new List<GameScreen>();
        readonly List<GameScreen> _screensToUpdate = new List<GameScreen>();
        readonly InputState _input = new InputState();

        SpriteBatch _spriteBatch;
        SpriteFont _font;

        public SpriteBatch SpriteBatch { get { return _spriteBatch; } }
        public SpriteFont Font { get { return _font; } }

        private static ScreenManager _instance;

        public static void Init(Microsoft.Xna.Framework.Game game)
        {
            if(_instance == null)
                _instance = new ScreenManager(game);
            else
            {
                throw new Exception("Already initialized");
            }
        }

        private ScreenManager(Microsoft.Xna.Framework.Game game)
            : base(game)
        {

        }

        public static ScreenManager Instance
        {
            get { return _instance; }
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = content.Load<SpriteFont>("menufont");
            foreach (GameScreen screen in _screens)
            {
                screen.LoadContent(); 
            }

            _gui = new GuiManager(Game.Services) { Visible = false };
            _inputManager = new InputManager(Game.Services, Game.Window.Handle);
            Game.Components.Add(_gui);
            Game.Components.Add(_inputManager);
        }
        protected override void UnloadContent()
        {
            foreach (GameScreen screen in _screens)
            {
                screen.UnloadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            _input.Update();

            _screensToUpdate.Clear();
            foreach (GameScreen screen in _screens)
                _screensToUpdate.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            while (_screensToUpdate.Count > 0)
            {
                GameScreen screen = _screensToUpdate[_screensToUpdate.Count - 1];
                _screensToUpdate.RemoveAt(_screensToUpdate.Count - 1);

                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(_input);

                        otherScreenHasFocus = true;
                    }

                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in _screens)
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
            _screens.Add(screen);
        }

        public void RemoveScreen(GameScreen screen)
        {
            screen.UnloadContent();
            _screens.Remove(screen);
            _screensToUpdate.Remove(screen);
        }

        public GameScreen[] GetScreens()
        {
            return _screens.ToArray();
        }

    }
}
