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
	class Remedy : AGameBonus
	{
		public Remedy(Vector2 coordinates)
			: base(coordinates)
		{
			this.Type = AGameObject.EnumObjectType.Remedy;
			this.damageFactor = 0.25f;
			this._milliseconds = Constants.REMEDY_MILLISECONDS; // redundant
		}
	}
}
