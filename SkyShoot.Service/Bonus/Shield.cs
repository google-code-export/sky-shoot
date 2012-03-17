using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkyShoot.Service.Bonus
{
	public class Shield : ABonus
	{
		public Shield(Guid id, DateTime startTime)
			: base(id, 0, 1, 30000, startTime,AObtainableDamageModifiers.Shield) {	}
	}
}