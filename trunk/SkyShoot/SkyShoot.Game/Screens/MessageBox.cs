using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using SkyShoot.Game.Client.Game;
using SkyShoot.Game.Controls;

namespace SkyShoot.Game.Screens
{
	internal class MessageBox : GameScreen
	{
		private readonly SoundManager _soundManager;
		private readonly ContentManager _content;

        private Texture2D _texture;

		private ButtonControl _okButton;

		public MessageBox()
		{
			CreateControls();
			InitializeControls();

			SoundManager.Initialize();
			_soundManager = SoundManager.Instance;
			_content = new ContentManager(ScreenManager.Instance.Game.Services, "Content");
		}

        public static ScreenManager.ScreenEnum Next { get; set; }

        public static string Message { get; set; }

        public override bool IsMenuScreen
        {
            get { return false; }
        }

		public override void LoadContent()
		{
			_texture = _content.Load<Texture2D>("Textures/screens/message_box");
		}

		public override void UnloadContent()
		{
			_content.Unload();
		}

		public void OkButtonPressed(object sender, EventArgs e)
		{
			_soundManager.SoundPlay(SoundManager.SoundEnum.Click);

			ScreenManager.Instance.SetActiveScreen(Next); // = Next;
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch spriteBatch = ScreenManager.Instance.SpriteBatch;

			SpriteFont font = ScreenManager.Instance.Font;
			Viewport viewport = ScreenManager.Instance.GraphicsDevice.Viewport;
			var viewportSize = new Vector2(viewport.Width, viewport.Height);
			Vector2 textSize = font.MeasureString(Message);
		    Vector2 textPosition = (viewportSize - textSize) / 2;
		    var backgroundRectangle = new Rectangle(0, 0, (int)viewportSize.X, (int)viewportSize.Y);

		    spriteBatch.Begin();
			spriteBatch.Draw(_texture, backgroundRectangle, Color.White);
			spriteBatch.DrawString(font, Message, textPosition, Color.White);
			spriteBatch.End();
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
