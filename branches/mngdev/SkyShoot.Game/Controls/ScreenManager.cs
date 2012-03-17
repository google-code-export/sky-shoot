using System;

using System.Collections.Generic;

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

		public Controller Controller
		{
			get { return _controller; }
		}

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

		public int Height
		{
			get { return GraphicsDevice.Viewport.Height; }
		}

		public int Width
		{
			get { return GraphicsDevice.Viewport.Width; }
		}

		private static ScreenManager _instance;

		private GameScreen _activeScreen;

		private readonly Dictionary<Type, GameScreen> _screens = new Dictionary<Type, GameScreen>();

		public void RegisterScreen(GameScreen gameScreen)
		{
			if (!_screens.ContainsKey(gameScreen.GetType()))
			{
				Type gameScreenType = gameScreen.GetType();
				_screens.Add(gameScreenType, gameScreen);
			}
			else
			{
				throw new Exception("game screen is already initialized");
			}
		}

		public void SetActiveScreen(Type gameScreenType)
		{
			if (_screens.ContainsKey(gameScreenType))
			{
				_activeScreen = _screens[gameScreenType];
				_gui.Screen = _activeScreen;
				_activeScreen.LoadContent();
			}
			else
			{
				throw new Exception("Game screen not found");
			}
		}

		public GameScreen GetActiveScreen()
		{
			return _activeScreen;
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

			if (Settings.Default.IsGamepad)
				_controller = new Gamepad(_inputManager);
			else
				_controller = new KeyboardAndMouse(_inputManager);
		}

		public static ScreenManager Instance
		{
			get { return _instance; }
		}

		// todo rewrite!
		protected override void LoadContent()
		{
			ContentManager content = Game.Content;
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			_font = content.Load<SpriteFont>("menufont");

			RegisterScreen(new LoginScreen());
			RegisterScreen(new MessageBox());
			RegisterScreen(new MainMenuScreen());
			RegisterScreen(new OptionsMenuScreen());
			RegisterScreen(new NewAccountScreen());
			RegisterScreen(new MultiplayerScreen());
			RegisterScreen(new CreateGameScreen());
			RegisterScreen(new WaitScreen());
			RegisterScreen(new LoadingScreen());
			RegisterScreen(new GameplayScreen());
			RegisterScreen(new GameMenuScreen());
		}

		protected override void UnloadContent()
		{
			foreach (GameScreen screen in _screens.Values)
			{
				screen.UnloadContent();
			}
		}

		public override void Update(GameTime gameTime)
		{
			_controller.Update();

			_activeScreen.HandleInput(_controller);
			_activeScreen.Update(gameTime);
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
	}
}
