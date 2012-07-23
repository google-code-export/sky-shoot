
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using SkyShoot.Game.Input;
using Color = Microsoft.Xna.Framework.Color;

namespace SkyShoot.Game.Screens
{
	public abstract class GameScreen : Screen
	{
		protected SpriteBatch SpriteBatch { get; private set; }

		protected ContentManager ContentManager { get; private set; }

		protected SpriteFont SpriteFont { get; set; }

		protected GameScreen()
		{
			Desktop.Bounds = new UniRectangle(
				new UniScalar(0.1f, 0.0f),
				new UniScalar(0.1f, 0.0f),
				new UniScalar(0.8f, 0.0f),
				new UniScalar(0.8f, 0.0f));

			Height = ScreenManager.Instance.Height;
			Width = ScreenManager.Instance.Width;

			SpriteBatch = ScreenManager.Instance.SpriteBatch;

			ContentManager = ScreenManager.Instance.ContentManager;

			SpriteFont = ScreenManager.Instance.Font;
		}

		public bool IsActive { get; set; }

		public virtual void LoadContent()
		{
		}

		public virtual void UnloadContent()
		{
		}

		public virtual void Update(GameTime gameTime)
		{
		}

		public virtual void HandleInput(Controller controller)
		{
		}

		public virtual void Draw(GameTime gameTime)
		{
		}

		protected void DrawString(string text, float positionX, float positionY, Color color)
		{
			SpriteBatch.DrawString(
				SpriteFont,
				text,
				new Vector2(positionX, positionY),
				color, 0, new Vector2(0f, 0f), 0.8f, SpriteEffects.None,
				layerDepth: 1f);
		}
	}
}
