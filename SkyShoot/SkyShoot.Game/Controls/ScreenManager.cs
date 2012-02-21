using System;

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Nuclex.Input;
using Nuclex.UserInterface;
using SkyShoot.Game.Screens;

namespace SkyShoot.Game.Controls
{
	public class ScreenManager : DrawableGameComponent
	{
		private readonly GuiManager _gui;
		private readonly InputManager _inputManager;

		private readonly Controller _controller;

		public GuiManager Gui
		{
			get { return _gui; }
		}

		private readonly List<GameScreen> _screens = new List<GameScreen>();

		private SpriteBatch _spriteBatch;
		private SpriteFont _font;

		public SpriteBatch SpriteBatch
		{
			get { return _spriteBatch; }
		}

		public SpriteFont Font
		{
			get { return _font; }
		}

		public int Height { get { return GraphicsDevice.Viewport.Height; } }

		public int Width
		{
			get { return GraphicsDevice.Viewport.Width; }
		}

		private static ScreenManager _instance;

		public enum ScreenEnum
		{
			LoginScreen,
			MessageScreen,
			MainMenuScreen,
			OptionsScreen,
			NewAccountScreen,
			MultiplayerScreen,
			CreateGameScreen,
			WaitScreen,
			LoadingScreen,
			GameplayScreen
		}

		private LoginScreen _loginScreen;
		private MessageBox _messageScreen;
		private MainMenuScreen _mainMenuScreen;
		private OptionsMenuScreen _optionsScreen;
		private NewAccountScreen _newAccountScreen;
		private MultiplayerScreen _multiplayerScreen;
		private CreateGameScreen _createGameScreen;
		private WaitScreen _waitScreen;
		private LoadingScreen _loadingScreen;
		private GameplayScreen _gameplayScreen;

		private ScreenEnum _activeScreen;

		// todo rewrite!
		public ScreenEnum ActiveScreen
		{
			get { return _activeScreen; }
			set
			{
				_activeScreen = value;
				foreach (GameScreen screen in _screens)
				{
					if (screen == null) return;
					screen.IsActive = false;
				}
				switch (_activeScreen)
				{
					case ScreenEnum.LoginScreen:
						_loginScreen.IsActive = true;
						_loginScreen.LoadContent();
						break;
					case ScreenEnum.MessageScreen:
						_messageScreen.IsActive = true;
						_messageScreen.LoadContent();
						break;
					case ScreenEnum.MainMenuScreen:
						_mainMenuScreen.IsActive = true;
						_mainMenuScreen.LoadContent();
						break;
					case ScreenEnum.OptionsScreen:
						_optionsScreen.IsActive = true;
						_optionsScreen.LoadContent();
						break;
					case ScreenEnum.NewAccountScreen:
						_newAccountScreen.IsActive = true;
						_newAccountScreen.LoadContent();
						break;
					case ScreenEnum.MultiplayerScreen:
						_multiplayerScreen.IsActive = true;
						_multiplayerScreen.LoadContent();
						break;
					case ScreenEnum.CreateGameScreen:
						_createGameScreen.IsActive = true;
						_createGameScreen.LoadContent();
						break;
					case ScreenEnum.WaitScreen:
						_waitScreen.IsActive = true;
						_waitScreen.LoadContent();
						break;
					case ScreenEnum.LoadingScreen:
						_loadingScreen.IsActive = true;
						_loadingScreen.LoadContent();
						break;
					case ScreenEnum.GameplayScreen:
						_gameplayScreen.IsActive = true;
						_gameplayScreen.LoadContent();
						break;
				}
			}
		}

		// todo temporary
		public GameScreen CurrentScreen
		{
			get
			{
				return _screens.FirstOrDefault(gameScreen => gameScreen.IsActive);
			}
		}

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
			_gui = new GuiManager(Game.Services) {Visible = false};
			_inputManager = new InputManager(Game.Services, Game.Window.Handle);
			Game.Components.Add(_gui);
			Game.Components.Add(_inputManager);
			
			_controller = new KeyboardAndMouse(_inputManager);
			//_controller = new Gamepad(_inputManager);
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

			_loginScreen = new LoginScreen();
			_screens.Add(_loginScreen);
			_messageScreen = new MessageBox();
			_screens.Add(_messageScreen);
			_mainMenuScreen = new MainMenuScreen();
			_screens.Add(_mainMenuScreen);
			_optionsScreen = new OptionsMenuScreen();
			_screens.Add(_optionsScreen);
			_newAccountScreen = new NewAccountScreen();
			_screens.Add(_newAccountScreen);
			_multiplayerScreen = new MultiplayerScreen();
			_screens.Add(_multiplayerScreen);
			_createGameScreen = new CreateGameScreen();
			_screens.Add(_createGameScreen);
			_waitScreen = new WaitScreen();
			_screens.Add(_waitScreen);
			_loadingScreen = new LoadingScreen();
			_screens.Add(_loadingScreen);
			_gameplayScreen = new GameplayScreen();
			_screens.Add(_gameplayScreen);

			foreach (GameScreen screen in _screens)
			{
				screen.LoadContent();
			}
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
			//_inputState.Update();
			_controller.Update();

			foreach (GameScreen screen in _screens)
			{
				if (screen.IsActive)
				{
					screen.HandleInput(_controller);
					screen.Update(gameTime);
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			foreach (GameScreen screen in _screens)
			{
				if (screen.IsActive == false)
					continue;

				screen.Draw(gameTime);
			}
		}

		public GameScreen[] GetScreens()
		{
			return _screens.ToArray();
		}

		public Vector2 GetMousePosition()
		{
			return _controller.SightPosition;
		}
	}
}
