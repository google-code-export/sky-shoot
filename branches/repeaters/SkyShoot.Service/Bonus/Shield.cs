using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkyShoot.Contracts.Bonuses;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Bonus
{
	public class Shield : AGameBonus
	{
		//public Shield(Guid id, DateTime startTime)
		//    : base(id, 0, 1, 30000, startTime, AObtainableDamageModifier.AObtainableDamageModifiers.Shield) {	}
		public Shield(Vector2 coordinates) : base(coordinates) { }
	}
}