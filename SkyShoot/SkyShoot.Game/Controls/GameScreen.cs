using Microsoft.Xna.Framework;

using Nuclex.UserInterface;

namespace SkyShoot.Game.Controls
{
	public abstract class GameScreen : Screen
	{
		public bool OtherScreenHasFocus;

		public bool IsActive { get; set; }

		public abstract bool IsMenuScreen { get; }

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
