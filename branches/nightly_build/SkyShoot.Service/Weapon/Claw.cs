using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Contracts;

namespace SkyShoot.Service.Weapon
{
	class Claw:AWeapon
	{
		public Claw(Guid id) : base(id) { Owner = null; }

		public Claw(Guid id, Contracts.Mobs.AMob owner)
			: base(id) 
		{
			this.Owner = owner;
			_reloadSpeed = Constants.CLAW_ATTACK_RATE;
		}

		public override Contracts.Weapon.Projectiles.AProjectile[] CreateBullets(Contracts.Mobs.AMob owner, Microsoft.Xna.Framework.Vector2 direction)
		{
			throw new NotImplementedException();
		}
	}
}
