using System;
using SkyShoot.Contracts;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon
{
	public class FlamePistol : AWeapon
	{
		public FlamePistol(Guid id, AGameObject owner = null) : base(id, owner)
		{
			WeaponType = WeaponType.FlamePistol;
			ReloadSpeed = Constants.FLAME_ATTACK_RATE;
		}

		public override AGameObject[] CreateBullets(Vector2 direction)
		{
			var step = Owner.ShootVector * Constants.FLAME_RADIUS / 2; // / Constants.FLAME_BULLETS_COUNT;
			var bullets = new Flame[Constants.FLAME_BULLETS_COUNT];
			for (var i = 0; i < Constants.FLAME_BULLETS_COUNT; i++)
			{
				var bulletCoordinates = Owner.Coordinates + i * step;
				bullets[i] = new Flame(Owner, Guid.NewGuid(), direction, bulletCoordinates);
			}
			return bullets;
		}
	}
}
