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
	public class FlamePistol : AWeapon
	{
		public FlamePistol(Guid id) : base(id)
		{
			WeaponType = AWeaponType.FlamePistol;
			ReloadSpeed = Constants.FLAME_ATTACK_RATE;
		}

		public FlamePistol(Guid id, AGameObject owner) : base(id, owner)
		{
			WeaponType = AWeaponType.FlamePistol;
			ReloadSpeed = Constants.FLAME_ATTACK_RATE;
		}

		public override AGameObject[] CreateBullets(AGameObject owner, Vector2 direction)
		{
			Vector2 step = owner.ShootVector / Constants.FLAME_BULLETS_COUNT;
			Flame[] bullets = new Flame[Constants.FLAME_BULLETS_COUNT];
			for (int i = 0; i < Constants.FLAME_BULLETS_COUNT; i ++)
			{
				Vector2 bulletCoordinates = owner.Coordinates + i * step;
				bullets[i] = new Flame(owner, Guid.NewGuid(), direction, bulletCoordinates);
			}
			return bullets;
		}
	}
}
