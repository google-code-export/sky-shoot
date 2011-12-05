using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.UserInterface;

namespace SkyShoot.Game.Screens
{
    class MessageBox:ScreenManager.GameScreen
    {
        //private readonly string _message;
		private GuiManager gui;
        private Texture2D _texture;
		private ContentManager _content;
		private ButtonControl okButton;
		private Nuclex.UserInterface.Screen messageScreen;

		public enum NextScreen
		{
			LoginScreen,
			NewAccountScreen
		}

		public ScreenManager.ScreenManager.ScreenEnum Next { get; set; }

        public MessageBox()
        {
            //_message = message;
			TransitionOnTime = TimeSpan.FromSeconds(0.2);
			TransitionOffTime = TimeSpan.FromSeconds(0.2);
			IsPopup = true;
        }

		public static String Message { get; set; }

        public override void LoadContent()
        {
            base.LoadContent();
			gui = ScreenManager.ScreenManager.Instance.Gui;
			Viewport viewport = ScreenManager.ScreenManager.Instance.GraphicsDevice.Viewport;
			messageScreen = new Nuclex.UserInterface.Screen(viewport.Width, viewport.Height);
			gui.Screen = messageScreen;
			if (_content == null)
				_content = new ContentManager(ScreenManager.ScreenManager.Instance.Game.Services, "Content");
			_texture = _content.Load<Texture2D>("Textures/screens/message_box");
			messageScreen.Desktop.Bounds = new UniRectangle(
				new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
				new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
			);

			okButton = new ButtonControl();
			okButton.Text = "Ok";
			okButton.Bounds = new UniRectangle(new UniScalar(0.5f, -50), new UniScalar(0.8f, -70), 100, 30);
			okButton.Pressed += OkButtonPressed;
			messageScreen.Desktop.Children.Add(okButton);
			
        }

		public void OkButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.ScreenManager.Instance.ActiveScreen = this.Next;
		}

		//public override void HandleInput(ScreenManager.InputState input)
		//{
		//    if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter)) ScreenManager.ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenManager.ScreenEnum.LoginScreen; 
		//}

        public override void Draw(GameTime gameTime)
        {			
			SpriteBatch spriteBatch = ScreenManager.ScreenManager.Instance.SpriteBatch;
			SpriteFont font = ScreenManager.ScreenManager.Instance.Font;
			Viewport viewport = ScreenManager.ScreenManager.Instance.GraphicsDevice.Viewport;
            var viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(Message);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            //const int hPad = 32;
            //const int vPad = 16;

            var backgroundRectangle = new Rectangle(0, 0, (int)viewportSize.X, (int)viewportSize.Y);
            //Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();

			//spriteBatch.Draw(_texture,
			//     new Rectangle(0, 0, viewport.Width, viewport.Height),
			//     Color.Gray);

            spriteBatch.Draw(_texture, backgroundRectangle, Color.White);

            spriteBatch.DrawString(font, Message, textPosition, Color.White);

            spriteBatch.End();

			base.Draw(gameTime);
			gui.Draw(gameTime);
        }
    }
}
