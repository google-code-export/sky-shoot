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
		private int _milliseconds; // @Sergey Terechenko : time = health
		private DateTime _startTime;

		public AGameBonus(Vector2 coordinates) : base(coordinates, Guid.NewGuid()) { }

		public AGameBonus(): base(Vector2.Zero, Guid.NewGuid()) { }

		public bool IsExpired(DateTime time)
		{
			return (time.CompareTo(_startTime.AddMilliseconds(_milliseconds)) == 1);
		}
	}               
}
