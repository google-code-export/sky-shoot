using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

namespace SkyShoot.Game.Screens
{
	internal class MessageBox : GameScreen
	{
		private Texture2D _texture;

		private ButtonControl _okButton;

		public MessageBox()
		{
			CreateControls();
			InitializeControls();
		}

		public static ScreenManager.ScreenEnum Next { get; set; }

		public static string Message { get; set; }

		public override void LoadContent()
		{
			_texture = ContentManager.Load<Texture2D>("Textures/screens/message_box");
		}

		public override void UnloadContent()
		{
			ContentManager.Unload();
		}

		public void OkButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(Next); // = Next;
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteFont font = ScreenManager.Instance.Font;
			Viewport viewport = ScreenManager.Instance.GraphicsDevice.Viewport;
			var viewportSize = new Vector2(viewport.Width, viewport.Height);
			Vector2 textSize = font.MeasureString(Message);
			Vector2 textPosition = (viewportSize - textSize) / 2;
			var backgroundRectangle = new Rectangle(0, 0, (int)viewportSize.X, (int)viewportSize.Y);

			SpriteBatch.Begin();
			SpriteBatch.Draw(_texture, backgroundRectangle, Color.White);
			SpriteBatch.DrawString(font, Message, textPosition, Color.White);
			SpriteBatch.End();
		}

		private void CreateControls()
		{
			_okButton = new ButtonControl
							{
								Text = "Ok",
								Bounds = new UniRectangle(new UniScalar(0.5f, -50), new UniScalar(0.8f, -70), 100, 30)
							};
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_okButton);

			ScreenManager.Instance.Controller.AddListener(_okButton, OkButtonPressed);
		}
	}
}
