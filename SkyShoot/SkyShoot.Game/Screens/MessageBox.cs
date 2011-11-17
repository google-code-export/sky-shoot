using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkyShoot.Game.Screens
{
    class MessageBox:ScreenManager.GameScreen
    {
        private string message;
        private Texture2D texture;
        public MessageBox(string text)
        {
            message = text;
            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
            IsPopup = true;
        }
        public override void LoadContent()
        {
            base.LoadContent();
            texture = new Texture2D(ScreenManager.Game.GraphicsDevice, 1, 1);
            Color[] colors = new Color[1];
            colors[0] = Color.LightSeaGreen;
            texture.SetData<Color>(colors);
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
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;


            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                          (int)textPosition.Y - vPad,
                                                          (int)textSize.X + hPad * 2,
                                                          (int)textSize.Y + vPad * 2);
            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();

            spriteBatch.Draw(texture,
                 new Rectangle(0, 0, viewport.Width, viewport.Height),
                 Color.Black * TransitionAlpha * 0.66f);

            spriteBatch.Draw(texture, backgroundRectangle, color);

            spriteBatch.DrawString(font, message, textPosition, color);

            spriteBatch.End();
        }
    }
}
