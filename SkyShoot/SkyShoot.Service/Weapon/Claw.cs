using System;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Contracts;
using SkyShoot.Contracts.Weapon.Projectiles;

namespace SkyShoot.Service.Weapon
{
	//!!@todo 2 delete
	class Claw:AWeapon
	{
		public Claw(Guid id) : base(id) { Owner = null; }

		public Claw(Guid id, Contracts.Mobs.AGameObject owner)
			: base(id) 
		{
			Owner = owner;
			ReloadSpeed = Constants.CLAW_ATTACK_RATE;
		}

		public override AProjectile[] CreateBullets(Contracts.Mobs.AGameObject owner, SkyShoot.XNA.Framework.Vector2 direction)
		{
			throw new NotImplementedException();
		}
	}
}
