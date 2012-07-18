using System;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon
{
	public class TurretMaker : AWeapon
	{
		public TurretMaker(Guid id, AGameObject owner = null)
			: base(id, owner)
		{
			WeaponType = WeaponType.TurretMaker;
			ReloadSpeed = Constants.TURRET_MAKER_ATTACK_RATE;
		}

		public override AGameObject[] CreateBullets(Vector2 direction)
		{
			Vector2 indent = Vector2.Multiply(Vector2.Normalize(Owner.ShootVector), Owner.Radius + Constants.TURRET_RADIUS);
			Vector2 turretPosition = Vector2.Add(Owner.Coordinates, indent);
			AWeapon weapon = new TurretGun(Guid.NewGuid(), turretPosition, Owner);
			var turret = new Turret(Constants.TURRET_HEALTH, weapon, Constants.TURRET_SHOOTING_DELAY, Owner, turretPosition);
			return new AGameObject[] { turret };
		}
	}
}
