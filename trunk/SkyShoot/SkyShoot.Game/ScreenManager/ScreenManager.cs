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
		private readonly GuiManager _gui;
		private readonly InputManager _inputManager;

	    private readonly InputState _inputState;

		public GuiManager Gui { get { return _gui; } }

		private readonly List<GameScreen> _screens = new List<GameScreen>();
		private readonly List<GameScreen> _screensToUpdate = new List<GameScreen>();

		private SpriteBatch _spriteBatch;
		private SpriteFont _font;

		public SpriteBatch SpriteBatch { get { return _spriteBatch; } }
		public SpriteFont Font { get { return _font; } }

		private static ScreenManager _instance;

		public bool IsActive { get; set; }

		/*private ScreenEnum _activeScreen;

		public ScreenEnum ActiveScreen
		{
		    get { return _activeScreen; }
		    set
		    { 
		        _activeScreen = value;
		        foreach (GameScreen screen in _screens)
		        {
		            screen.IsActive = false;
		        }
		        switch(_activeScreen)
		        {
		            case LoginScreen:
		                LoginScreen.IsActive = true;
		            case MessageScreen:
		                MessageBox.IsActive = true;
		            case MainMenuScreen:
		                MainMenuScreen.IsActive = true;
		            case OptionsMenuScreen:
		                OptionsMenuScreen.IsActive = true;
		            case NewAccountScreen:
		                NewAccountScreen.IsActive = true;
		            case MultiplayerScreen:
		                MultiplayerScreen.IsActive = true;
		            case CreateGameScreen:
		                CreateGameScreen.IsActive = true;
		            case WaitScreen:
		                WaitScreen.IsActive = true;
		            case LoadingScreen:
		                LoadingScreen.IsActive = true;
		            case GameplayScreen:
		                GameplayScreen.IsActive = true;
		        }
		        }
		}

		enum ScreenEnum { LoginScreen, MessageScreen, MainMenuScreen, OptionsMenuScreen, NewAccountScreen, MultiplayerScreen, CreateGameScreen, WaitScreen, LoadingScreen, GameplayScreen }
        */
		public static void Init(Microsoft.Xna.Framework.Game game)
		{
			if (_instance == null)
				_instance = new ScreenManager(game);
			else
			{
				throw new Exception("Already initialized");
			}
		}

		private ScreenManager(Microsoft.Xna.Framework.Game game)
			: base(game)
		{
            _gui = new GuiManager(Game.Services) { Visible = false };
            _inputManager = new InputManager(Game.Services, Game.Window.Handle);
            Game.Components.Add(_gui);
            Game.Components.Add(_inputManager);

            _inputState = new InputState(_inputManager);
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

			//_screens.Add(new LoginScreen());
			//_screens.Add(new MessageBox());
			//_screens.Add(new MainMenuScreen());
			//_screens.Add(new NewAccountScreen());
			//_screens.Add(new MultiplayerScreen());
			//_screens.Add(new CreateGameScreen());
			//_screens.Add(new WaitScreen(WaitScreen._tile.Text, _gameMode.Text, _maxPlayers.Text, GameController.Instance.GetGameList().Length));
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
			_inputState.Update();

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
						screen.HandleInput(_inputState);

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

        public void GetMouseState(out float x, out float y)
        {
            x = _inputState.CurrentMouseState.X;
            y = _inputState.CurrentMouseState.Y;
        }

	}
}
