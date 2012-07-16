using System;
using SkyShoot.Contracts;
using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon.Bullets
{
	public class TurretGunBullet : AProjectile
	{
		public TurretGunBullet(AGameObject owner, Guid id, Vector2 direction, Vector2 turretPosition)
			: base(owner, id, direction, turretPosition)
		{
			Speed = Constants.TURRET_GUN_BULLET_SPEED;
			Damage = Constants.TURRET_GUN_BULLET_DAMAGE;
			HealthAmount = Constants.TURRET_GUN_BULLET_LIFE_DISTANCE;
			ObjectType = EnumObjectType.TurretGunBullet;
		}
	}
}
