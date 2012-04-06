using System;

using System.Diagnostics;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Game.Client.View;
using SkyShoot.Game.Controls;
using SkyShoot.Game.Screens;
using SkyShoot.Game.Client.Game;

namespace SkyShoot.Game
{
	public class SkyShootGame : Microsoft.Xna.Framework.Game
	{
		private ScreenManager _screenManager;
		private SpriteBatch _spriteBatch;
		private readonly GraphicsDeviceManager _graphics;
		private SoundManager _soundManager;


		public SkyShootGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";			
			bool fscreen = Settings.Default.FullScreenSelected;
			switch (fscreen)
			{
				case false:
					{
						_graphics.PreferredBackBufferWidth = 800;
						_graphics.PreferredBackBufferHeight = 600;
					}
					break;
				case true:
					_graphics.IsFullScreen = true;
					break;
			}
			IsMouseVisible = false;

		}

		protected override void Initialize()
		{
			Textures.GraphicsDevice = GraphicsDevice;
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			ScreenManager.Init(this);
			_screenManager = ScreenManager.Instance;
			Components.Add(_screenManager);

			SoundManager.Initialize();
			_soundManager = SoundManager.Instance;
			_soundManager.SoundPlay(7);

			base.Initialize();

			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.LoginScreen);
		}

		protected override void LoadContent()
		{
			Textures.Arrow = Content.Load<Texture2D>("Textures/Cursors/Arrow");
			Textures.Plus = Content.Load<Texture2D>("Textures/Cursors/Plus");
			Textures.Cross = Content.Load<Texture2D>("Textures/Cursors/Cross");
			Textures.Target = Content.Load<Texture2D>("Textures/Cursors/Target");
		}

		protected override void UnloadContent()
		{

		}

		protected override void Update(GameTime gameTime)
		{
			try
			{
				base.Update(gameTime);
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
			}
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			base.Draw(gameTime);
			_spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
			Vector2 mousePosition = ScreenManager.Instance.GetMousePosition();
			_spriteBatch.Draw(Textures.ActiveCursor, Textures.GetCursorPosition(mousePosition.X, mousePosition.Y), Color.White);
			_spriteBatch.End();
		}
	}
}
