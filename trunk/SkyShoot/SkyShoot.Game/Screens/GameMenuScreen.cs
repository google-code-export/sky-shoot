using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using SkyShoot.Game.Network;

namespace SkyShoot.Game.Screens
{
	internal class GameMenuScreen : GameScreen
	{
		private static Texture2D _texture;

		private ButtonControl _continueButton;
		private ButtonControl _optionsButton;
		private ButtonControl _exitButton;

		public GameMenuScreen()
		{
			CreateControls();
			InitializeControls();
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch.Begin();
			SpriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			SpriteBatch.End();
		}

		public override void LoadContent()
		{
			_texture = ContentManager.Load<Texture2D>("Textures/screens/screen_06");
		}

		public override void UnloadContent()
		{
			ContentManager.Unload();
		}

		private void CreateControls()
		{
			_continueButton = new ButtonControl
								{
									Text = "Continue",
									Bounds =
										new UniRectangle(
										new UniScalar(0.30f, 0),
										new UniScalar(0.2f, 0),
										new UniScalar(0.4f, 0),
										new UniScalar(0.1f, 0)),
								};

			_optionsButton = new ButtonControl
								{
									Text = "Options",
									Bounds =
										new UniRectangle(
										new UniScalar(0.30f, 0),
										new UniScalar(0.35f, 0),
										new UniScalar(0.4f, 0),
										new UniScalar(0.1f, 0)),
								};

			_exitButton = new ButtonControl
							{
								Text = "Exit",
								Bounds =
									new UniRectangle(
									new UniScalar(0.30f, 0),
									new UniScalar(0.5f, 0),
									new UniScalar(0.4f, 0),
									new UniScalar(0.1f, 0)),
							};
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_continueButton);
			Desktop.Children.Add(_optionsButton);
			Desktop.Children.Add(_exitButton);

			ScreenManager.Instance.Controller.AddListener(_continueButton, ContinueButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_optionsButton, OptionsButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_exitButton, ExitButtonPressed);
		}

		private void ContinueButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.GameplayScreen);
		}

		private void OptionsButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.OptionsMenuScreen);
		}

		private void ExitButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(ScreenManager.ScreenEnum.LoginScreen);
			ConnectionManager.Instance.LeaveGame();
		}
	}
}
