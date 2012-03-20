using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SkyShoot.Contracts.Bonuses;

namespace SkyShoot.Service.Bonus
{
	public abstract class ABonus : AObtainableDamageModifier
	{
		protected ABonus(Guid id, float inDamage, float outDamage, int milliseconds, DateTime startTime,AObtainableDamageModifiers type)
			: base(id)
		{
			Type = type;
			this.InDamage = inDamage;
			this.OutDamage = outDamage;

			_milliseconds = milliseconds;
			_startTime = startTime;
		}

		public float InDamage { get; private set; }
		public float OutDamage { get; private set; }

		private int _milliseconds;

		private DateTime _startTime;

		public bool IsExpired(DateTime time)
		{
			return (time.CompareTo(_startTime.AddMilliseconds(_milliseconds)) == 1);
		}
	}
}