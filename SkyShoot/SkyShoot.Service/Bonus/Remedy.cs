﻿using SkyShoot.Contracts.Service;
using SkyShoot.Service.Bonuses;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Bonus
{
	public class Remedy : AGameBonus
	{
		public Remedy(Vector2 coordinates)
			: base(coordinates)
		{
			ObjectType = EnumObjectType.Remedy;
			DamageFactor = 0.25f;
			Milliseconds = Constants.REMEDY_MILLISECONDS; // redundant
		}
	}
}
