using Microsoft.Xna.Framework;

using Nuclex.UserInterface;

namespace SkyShoot.Game.Controls
{
	public abstract class GameScreen : Screen
	{
		public bool OtherScreenHasFocus;

		public bool IsActive { get; set; }

		public abstract bool IsMenuScreen { get; }

		protected GameScreen()
		{
			Desktop.Bounds = new UniRectangle(
				new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
				new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
				);

			Height = ScreenManager.Instance.Height;
			Width = ScreenManager.Instance.Width;			
		}

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
	}
}
