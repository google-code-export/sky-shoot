using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Input;
using Nuclex.UserInterface;
using SkyShoot.Game.Input;

namespace SkyShoot.Game.Screens
{
	public class ScreenManager : DrawableGameComponent
	{
		public enum ScreenEnum
		{
			CreateGameScreen,
			GameMenuScreen,
			GameplayScreen,
			LoadingScreen,
			LoginScreen,
			MainMenuScreen,
			MessageBoxScreen,
			MultiplayerScreen,
			NewAccountScreen,
			OptionsMenuScreen,
			WaitScreen
		}

		private static ScreenManager _instance;

		private readonly GuiManager _gui;
		private readonly InputManager _inputManager;
		private readonly Dictionary<ScreenEnum, GameScreen> _screens = new Dictionary<ScreenEnum, GameScreen>();

		private readonly Controller _controller;

		private SpriteBatch _spriteBatch;

		private SpriteFont _font;

		private GameScreen _activeScreen;

		private ScreenManager(Microsoft.Xna.Framework.Game game)
			: base(game)
		{
			_gui = new GuiManager(Game.Services) { Visible = false };
			_inputManager = new InputManager(Game.Services, Game.Window.Handle);

			Game.Components.Add(_gui);
			Game.Components.Add(_inputManager);

			if (Settings.Default.IsGamepad)
				_controller = new Gamepad(_inputManager);
			else
				_controller = new KeyboardAndMouse(_inputManager);
		}

		public static ScreenManager Instance
		{
			get { return _instance; }
		}

		public Controller Controller
		{
			get { return _controller; }
		}

		public SpriteBatch SpriteBatch
		{
			get { return _spriteBatch; }
		}

		public SpriteFont Font
		{
			get { return _font; }
		}

		public int Height
		{
			get { return GraphicsDevice.Viewport.Height; }
		}

		public int Width
		{
			get { return GraphicsDevice.Viewport.Width; }
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

		public void SetActiveScreen(ScreenEnum screenName)
		{
			if (_screens.ContainsKey(screenName))
			{
				_activeScreen = _screens[screenName];
				_gui.Screen = _activeScreen;
				_activeScreen.LoadContent();
			}
			else
			{
				throw new Exception("Game screen not found");
			}
		}

		public void RegisterScreen(ScreenEnum screenName, GameScreen gameScreen)
		{
			if (!_screens.ContainsKey(screenName))
			{
				_screens.Add(screenName, gameScreen);
			}
			else
			{
				throw new Exception("game screen is already initialized");
			}
		}

		public GameScreen GetActiveScreen()
		{
			return _activeScreen;
		}

		public override void Update(GameTime gameTime)
		{
			_activeScreen.Update(gameTime);

			_controller.Update();
			_activeScreen.HandleInput(_controller);
		}

		public override void Draw(GameTime gameTime)
		{
			_activeScreen.Draw(gameTime);
			_gui.Draw(gameTime);
		}

		// todo remove
		public Vector2 GetMousePosition()
		{
			return _controller.SightPosition;
		}

		// todo rewrite!
		protected override void LoadContent()
		{
			ContentManager content = Game.Content;
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			_font = content.Load<SpriteFont>("menufont");

			RegisterScreen(ScreenEnum.LoginScreen, new LoginScreen());
			RegisterScreen(ScreenEnum.MessageBoxScreen, new MessageBox());
			RegisterScreen(ScreenEnum.MainMenuScreen, new MainMenuScreen());
			RegisterScreen(ScreenEnum.OptionsMenuScreen, new OptionsMenuScreen());
			RegisterScreen(ScreenEnum.NewAccountScreen, new NewAccountScreen());
			RegisterScreen(ScreenEnum.MultiplayerScreen, new MultiplayerScreen());
			RegisterScreen(ScreenEnum.CreateGameScreen, new CreateGameScreen());
			RegisterScreen(ScreenEnum.WaitScreen, new WaitScreen());
			RegisterScreen(ScreenEnum.LoadingScreen, new LoadingScreen());
			RegisterScreen(ScreenEnum.GameplayScreen, new GameplayScreen());
			RegisterScreen(ScreenEnum.GameMenuScreen, new GameMenuScreen());
		}

		protected override void UnloadContent()
		{
			foreach (GameScreen screen in _screens.Values)
			{
				screen.UnloadContent();
			}
		}
	}
}