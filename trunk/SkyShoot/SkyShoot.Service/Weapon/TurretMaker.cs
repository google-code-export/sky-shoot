using System;
using SkyShoot.Contracts;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.XNA.Framework;
using System.Diagnostics;
using SkyShoot.Contracts.Service;

namespace SkyShoot.Service.Weapon
{
	public class TurretMaker : AWeapon
	{
		public TurretMaker(Guid id, AGameObject owner = null)
			: base(id, owner)
		{
			WeaponType = WeaponType.TurretMaker;
			ReloadSpeed = Constants.TURRET_GUN_ATTACK_RATE;
		}

		public override AGameObject[] CreateBullets(Vector2 direction)
		{
			Vector2 turretPosition = Vector2.Add(Owner.Coordinates, new Vector2(0, 45));
			AWeapon weapon = new TurretGun(Guid.NewGuid(), turretPosition, Owner);
			var turret = new Turret(Constants.TURRET_HEALTH, weapon, Constants.TURRET_SHOOTING_DELAY, Owner, turretPosition);
			return new AGameObject[] { turret };
		}
	}
}
