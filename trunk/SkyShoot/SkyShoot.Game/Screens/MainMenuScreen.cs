using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

using SkyShoot.Game.Controls;

namespace SkyShoot.Game.Screens
{
	internal class MainMenuScreen : GameScreen
	{
		private GuiManager _gui;

		private static Texture2D _texture;

		private ContentManager _content;

		private SpriteBatch _spriteBatch;

		public override bool IsMenuScreen
		{
			get { return true; }
		}

		public override void LoadContent()
		{
			base.LoadContent();
			_gui = ScreenManager.Instance.Gui;
			_gui.Screen = this;

			Height = ScreenManager.Instance.Height;
			Width = ScreenManager.Instance.Width;
			
			if (_content == null)
				_content = new ContentManager(ScreenManager.Instance.Game.Services, "Content");

			_texture = _content.Load<Texture2D>("Textures/screens/screen_05_fix");

			Desktop.Bounds = new UniRectangle(
				new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
				new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
				);

			var playGameButton = new ButtonControl
			                     	{
			                     		Text = "Multiplayer",
			                     		Bounds =
			                     			new UniRectangle(new UniScalar(0.30f, 0), new UniScalar(0.2f, 0),
			                     			                 new UniScalar(0.4f, 0), new UniScalar(0.1f, 0)),
			                     	};
			Desktop.Children.Add(playGameButton);		

			var optionsButton = new ButtonControl
			                    	{
			                    		Text = "Options",
			                    		Bounds =
			                    			new UniRectangle(new UniScalar(0.30f, 0), new UniScalar(0.35f, 0),
			                    			                 new UniScalar(0.4f, 0), new UniScalar(0.1f, 0)),
			                    	};
			Desktop.Children.Add(optionsButton);

			var logoffButton = new ButtonControl
			                   	{
			                   		Text = "Logoff",
			                   		Bounds =
			                   			new UniRectangle(new UniScalar(0.30f, 0), new UniScalar(0.5f, 0),
			                   			                 new UniScalar(0.4f, 0), new UniScalar(0.1f, 0)),
			                   	};
			Desktop.Children.Add(logoffButton);

			playGameButton.Pressed += PlayGameButtonPressed;
			optionsButton.Pressed += OptionsButtonPressed;
			logoffButton.Pressed += LogoffMenuButtonPressed;
		}

		private void PlayGameButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenEnum.MultiplayerScreen;
		}

		private void OptionsButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenEnum.OptionsScreen;
		}

		private void LogoffMenuButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenEnum.LoginScreen;
		}

		public override void Draw(GameTime gameTime)
		{
			_spriteBatch = ScreenManager.Instance.SpriteBatch;
			_spriteBatch.Begin();
			_spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			_spriteBatch.End();
			_gui.Draw(gameTime);
			base.Draw(gameTime);
		}
	}
}
