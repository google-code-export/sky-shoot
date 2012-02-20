using Nuclex.UserInterface.Controls;

namespace SkyShoot.Game.Controls
{
	public class Controller
	{
		public delegate void ButtonPressed(Control control);

		private GameScreen _activeScreen;

		void Update()
		{
			
		}

		void RegisterListener(Control control, ButtonPressed buttonPressed)
		{
			
		}
	}
}
