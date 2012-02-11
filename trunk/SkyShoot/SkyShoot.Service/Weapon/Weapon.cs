using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Bonuses.Weapon.Projectiles;
using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Service.Weapon
{
	public abstract class Weapon : ABonus
	{
		protected Weapon(Guid id) : base(id) { }

		public abstract AProjectile[] CreateBullets(AMob owner, PointF direction); 
	}
}