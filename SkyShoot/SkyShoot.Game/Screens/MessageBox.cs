using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SkyShoot.Game.Screens
{
    class MessageBox:ScreenManager.GameScreen
    {
        private readonly string _message;
        private Texture2D _texture;
		private ContentManager _content;

        public MessageBox(string message)
        {
            _message = message;
            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
            IsPopup = true;
        }
        public override void LoadContent()
        {
            base.LoadContent();
			if (_content == null)
				_content = new ContentManager(ScreenManager.Game.Services, "Content");
			_texture = _content.Load<Texture2D>("Textures/screens/message_box");
        }
        public override void HandleInput(ScreenManager.InputState input)
        {
            if (input.IsMenuSelect()||input.IsMenuCancel()) ExitScreen(); 
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            var viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(_message);
            Vector2 textPosition = (viewportSize - textSize) / 2;


            const int hPad = 32;
            const int vPad = 16;

            var backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                          (int)textPosition.Y - vPad,
                                                          (int)textSize.X + hPad * 2,
                                                          (int)textSize.Y + vPad * 2);
            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();

            spriteBatch.Draw(_texture,
                 new Rectangle(0, 0, viewport.Width, viewport.Height),
                 Color.Black * TransitionAlpha * 0.66f);

            spriteBatch.Draw(_texture, backgroundRectangle, color);

            spriteBatch.DrawString(font, _message, textPosition, color);

            spriteBatch.End();
        }
    }
}
