using System;
using Microsoft.Xna.Framework.Graphics;
using SkyShoot.Contracts.Bonuses;
using SkyShoot.Game.Client.View;

namespace SkyShoot.Game.Client.GameObjects
{
	public abstract class ABonus : AObtainableDamageModifier, IDrawable
	{
		private readonly DateTime _startTime;

		private readonly DateTime _endTime;

		protected ABonus(Guid id, int milliseconds, DateTime startTime)
			: base(id)
		{
			_startTime = startTime;
			_endTime = _startTime.AddMilliseconds(milliseconds);
		}

		public bool IsExpired(DateTime time)
		{
			return (time.CompareTo(_endTime) == 1);
		}

		public abstract void Draw(SpriteBatch spriteBatch);
	}
}
