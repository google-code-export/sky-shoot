using SkyShoot.Game.Controls;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkyShoot.Game.Screens
{
	internal class LoadingScreen : GameScreen
	{
		public static bool ShowLoadingMessage { get; set; }

		public override bool IsMenuScreen
		{
			get { return false; }
		}

		public override void Update(GameTime gameTime)
		{

		}

		public override void Draw(GameTime gameTime)
		{
			if (ShowLoadingMessage)
			{
				SpriteBatch spriteBatch = ScreenManager.Instance.SpriteBatch;
				SpriteFont font = ScreenManager.Instance.Font;
				const string message = "Loading...";
				Viewport viewport = ScreenManager.Instance.GraphicsDevice.Viewport;
				var viewportSize = new Vector2(viewport.Width, viewport.Height);
				Vector2 textSize = font.MeasureString(message);
				Vector2 textPosition = (viewportSize - textSize) / 2;
				spriteBatch.Begin();
				spriteBatch.DrawString(font, message, textPosition, Color.White);
				spriteBatch.End();
			}
		}
	}
}
