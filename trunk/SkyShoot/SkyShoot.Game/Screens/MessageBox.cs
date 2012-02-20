using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Nuclex.UserInterface;

using Nuclex.UserInterface.Controls.Desktop;
using SkyShoot.Game.Controls;

namespace SkyShoot.Game.Screens
{
	internal class MessageBox : GameScreen
	{
		private GuiManager _gui;

		private Texture2D _texture;

		private ContentManager _content;

		private ButtonControl _okButton;

		public ScreenManager.ScreenEnum Next { get; set; }

		public static String Message { get; set; }

		public override bool IsMenuScreen
		{
			get { return false; }
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

			_texture = _content.Load<Texture2D>("Textures/screens/message_box");

			Desktop.Bounds = new UniRectangle(
				new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
				new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
				);

			_okButton = new ButtonControl
			            	{
			            		Text = "Ok",
			            		Bounds = new UniRectangle(new UniScalar(0.5f, -50), new UniScalar(0.8f, -70), 100, 30)
			            	};
			_okButton.Pressed += OkButtonPressed;
			Desktop.Children.Add(_okButton);
		}

		public void OkButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.ActiveScreen = Next;
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch spriteBatch = ScreenManager.Instance.SpriteBatch;
			SpriteFont font = ScreenManager.Instance.Font;
			Viewport viewport = ScreenManager.Instance.GraphicsDevice.Viewport;
			var viewportSize = new Vector2(viewport.Width, viewport.Height);
			Vector2 textSize = font.MeasureString(Message);
			Vector2 textPosition = (viewportSize - textSize) / 2;
			var backgroundRectangle = new Rectangle(0, 0, (int) viewportSize.X, (int) viewportSize.Y);
			spriteBatch.Begin();
			spriteBatch.Draw(_texture, backgroundRectangle, Color.White);
			spriteBatch.DrawString(font, Message, textPosition, Color.White);
			spriteBatch.End();
			base.Draw(gameTime);
			_gui.Draw(gameTime);
		}
	}
}
