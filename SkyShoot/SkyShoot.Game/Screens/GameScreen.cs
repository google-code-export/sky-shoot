using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using SkyShoot.Game.Input;
using Color = Microsoft.Xna.Framework.Color;
using SkyShoot.Contracts.Service;

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

		/// <summary>
		/// Загрузка контента, необходимого
		/// для отображения экрана 
		/// </summary>
		public virtual void LoadContent()
		{
		}

		/// <summary>
		/// Уничтожение экрана, освобождение всех ресурсов
		/// </summary>
		public virtual void Destroy()
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

		/// <summary>
		/// Вызывается перед отображением экрана
		/// </summary>
		public virtual void OnShow()
		{
			
		}

		/// <summary>
		/// Вызывается при скрытии экрана
		/// </summary>
		public virtual void OnHide()
		{
			
		}

		protected void DrawString(string text, float positionX, float positionY, Color color)
		{
			SpriteBatch.DrawString(
				SpriteFont,
				text,
				new Vector2(positionX, positionY),
				color, 0, new Vector2(0f, 0f), 0.8f, SpriteEffects.None,
				layerDepth: Constants.TEXT_TEXTURE_LAYER);
		}
	}
}