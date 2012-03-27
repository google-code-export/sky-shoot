using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.Bonuses
{
	public class AGameBonus : AGameObject
	{
		protected int _milliseconds; // @Sergey Terechenko : time = health
		protected long _startTime;

		public float damageFactor;

		public AGameBonus(Vector2 coordinates) : base(coordinates, Guid.NewGuid()) { }

		public bool IsExpired(long time)
		{
			return (time - _startTime > _milliseconds);
		}

		public void taken(long startTime)
		{
			this._startTime = startTime;
		}
	}               
}
