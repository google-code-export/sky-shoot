using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SkyShoot.Contracts.Bonuses;

namespace SkyShoot.Service.Bonus
{
	public abstract class Bonus : ABonus
	{
		protected Bonus(Guid id, float inDamage, float outDamage, int milliseconds, DateTime startTime)
			: base(id)
		{
			this.inDamage = inDamage;
			this.outDamage = outDamage;

			_milliseconds = milliseconds;
			_startTime = startTime;
		}

		public float inDamage { get; private set; }
		public float outDamage { get; private set; }

		private int _milliseconds;

		private DateTime _startTime;

		public bool IsExpired(DateTime time)
		{
			return (time.CompareTo(_startTime.AddMilliseconds(_milliseconds)) == 1);
		}
	}
}