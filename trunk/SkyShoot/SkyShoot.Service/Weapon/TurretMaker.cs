using System;
using SkyShoot.Contracts;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.XNA.Framework;
using System.Diagnostics;

namespace SkyShoot.Service.Weapon
{
	public class TurretMaker : AWeapon
	{
		public TurretMaker(Guid id, AGameObject owner = null)
			: base(id, owner)
		{
			WeaponType = WeaponType.TurretMaker;
			//TODO: set reload speed
			ReloadSpeed = 10;
		}

		public override AGameObject[] CreateBullets(Vector2 direction)
		{
			Vector2 turretPosition = Vector2.Add(Owner.Coordinates, new Vector2(0, 45));
			AWeapon weapon = new TurretGun(Guid.NewGuid(), turretPosition, Owner);
			var turret = new Turret(100, weapon, 10, Owner, turretPosition);
			return new AGameObject[] { turret };
		}
	}
}
