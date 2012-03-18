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
		public AGameBonus(Vector2 coordinates) : base(coordinates, Guid.NewGuid()) { }
	}
}
