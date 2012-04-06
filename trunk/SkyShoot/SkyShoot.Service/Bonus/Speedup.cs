using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts;
using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Bonus
{
	public class Speedup : AGameBonus
	{
		public Speedup(Vector2 coordinates)
			: base(coordinates)
		{
			this.milliseconds = Constants.SPEEDUP_MILLISECONDS;
			this.ObjectType = EnumObjectType.Speedup;
			this.DamageFactor = 1.5f; // not damage, but speedup here
		}
	}
}
