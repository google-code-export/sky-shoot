using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Nuclex.Input;

namespace SkyShoot.Game.Controls
{
	public class InputState
	{
		private readonly InputManager _inputManager;

		public InputState(InputManager inputManager)
		{
			_inputManager = inputManager;
		}

		public void Update()
		{

		}

		public Vector2 RunVector(KeyboardState keyboardState, GamePadState gamePadState)
		{
			return new Vector2();
		}

		/*public bool IsNewKeyPressed(Keys key)
		{
			return (_currentKeyboardState.IsKeyDown(key) &&
			        _lastKeyState.IsKeyUp(key));
		}

		public bool IsNewButtonPressed(Buttons button)
		{
			return (_currentGamePadState.IsButtonDown(button) &&
			        _lastGamePadState.IsButtonDown(button));
		}

		public bool IsMenuSelect()
		{
			return IsNewKeyPressed(Keys.Space) ||
			       IsNewKeyPressed(Keys.Enter) ||
			       IsNewButtonPressed(Buttons.A) ||
			       IsNewButtonPressed(Buttons.Start);
		}

		public bool IsMenuCancel()
		{
			return IsNewKeyPressed(Keys.Escape) ||
			       IsNewButtonPressed(Buttons.B) ||
			       IsNewButtonPressed(Buttons.Back);
		}

		public bool IsMenuUp()
		{
			return IsNewKeyPressed(Keys.Up) ||
			       IsNewButtonPressed(Buttons.DPadUp) ||
			       IsNewButtonPressed(Buttons.LeftThumbstickUp);
		}

		public bool IsMenuDown()
		{
			return IsNewKeyPressed(Keys.Down) ||
			       IsNewButtonPressed(Buttons.DPadDown) ||
			       IsNewButtonPressed(Buttons.LeftThumbstickDown);
		}*/
	}
}
