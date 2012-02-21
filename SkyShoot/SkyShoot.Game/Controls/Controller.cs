using Microsoft.Xna.Framework;

using Nuclex.Input;
using Nuclex.UserInterface.Controls;

namespace SkyShoot.Game.Controls
{
	public abstract class Controller
	{
		public delegate void ButtonPressed(Control control);

		private GameScreen _activeScreen;
		protected readonly InputManager InputManager;

		protected Controller(InputManager inputManager)
		{
			InputManager = inputManager;
		}

		public abstract void Update();

		public abstract Vector2 RunVector { get; }

		public abstract Vector2 SightPosition { get; }

		public void RegisterListener(Control control, ButtonPressed buttonPressed)
		{
			
		}
	}
}
