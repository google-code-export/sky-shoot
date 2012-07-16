using System;
using SkyShoot.Contracts;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon
{
	class RocketPistol : AWeapon
	{
		public RocketPistol(Guid id, AGameObject owner = null) : base(id, owner)
		{
			WeaponType = WeaponType.RocketPistol;
			ReloadSpeed = Constants.ROCKET_PISTOL_ATTACK_RATE;
		}

		public override AGameObject[] CreateBullets(Vector2 direction)
		{
			var bullets = new[] { new RocketBullet(Owner, Guid.NewGuid(), direction) };
			return bullets;
		}
	}
}
