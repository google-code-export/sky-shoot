using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon
{
	class RocketPistol : AWeapon
	{
		public RocketPistol(Guid id) : base(id)
		{
			WeaponType = AWeaponType.RocketPistol;
			ReloadSpeed = Constants.ROCKET_PISTOL_ATTACK_RATE;
		}

		public RocketPistol(Guid id, AGameObject owner) : base(id, owner)
		{
			WeaponType = AWeaponType.RocketPistol;
			ReloadSpeed = Constants.ROCKET_PISTOL_ATTACK_RATE;
		}

		public override AGameObject[] CreateBullets(AGameObject owner, Vector2 direction)
		{
			var bullets = new[] { new RocketBullet(owner, Guid.NewGuid(), direction) };
			return bullets;
		}
	}
}
