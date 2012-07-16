using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts;
using SkyShoot.XNA.Framework;
using SkyShoot.Service.Weapon.Bullets;

namespace SkyShoot.Service.Weapon
{
	public class TurretGun : AWeapon
	{
		private Vector2 parentTurretCoordinates;

		public TurretGun(Guid id, Vector2 parentTurretCoordinates, AGameObject owner = null)
			: base(id, owner)
		{
			WeaponType = WeaponType.TurretGun;
			ReloadSpeed = Constants.TURRET_GUN_ATTACK_RATE;
			this.parentTurretCoordinates = parentTurretCoordinates;
		}

		public override AGameObject[] CreateBullets(Vector2 direction)
		{
			var bullets = new[] { new TurretGunBullet(Owner, Guid.NewGuid(), direction, parentTurretCoordinates) };
			return bullets;
		}
	}
}
