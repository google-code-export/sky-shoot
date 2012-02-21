using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Nuclex.Input;

namespace SkyShoot.Game.Controls
{
	class Gamepad : Controller
	{
		private GamePadState _currentGamePadState;
		private GamePadState _lastGamePadState;

		public Gamepad(InputManager inputManager) : base(inputManager)
		{
			// todo windows | XBOX
			_currentGamePadState = InputManager.GetGamePad(ExtendedPlayerIndex.Five).GetState();
		}

		public override void Update()
		{
			// todo wrong, 10%
			_lastGamePadState = _currentGamePadState;

			_currentGamePadState = InputManager.GetGamePad(ExtendedPlayerIndex.Five).GetState();			
		}

		public override Vector2? RunVector
		{
			get { throw new NotImplementedException(); }
		}

		public override Vector2 SightPosition
		{
			get { return new Vector2();}
		}

		public override ButtonState ShootButton
		{
			get { throw new NotImplementedException(); }
		}

		private Vector2 GetRunVector(GamePadState gamePadState)
		{
			Vector2 runVector = Vector2.Zero;
			if (gamePadState.IsConnected)
			{
				runVector = gamePadState.ThumbSticks.Left;
				runVector.Y = -runVector.Y;
			}
			return runVector;
		}
	}
}
