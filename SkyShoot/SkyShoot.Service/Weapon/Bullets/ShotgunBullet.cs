using System;

using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon.Projectiles;
using SkyShoot.XNA.Framework;
using SkyShoot.Contracts;

namespace SkyShoot.Service.Weapon.Bullets
{
	public class ShotgunBullet : AProjectile
	{
		public ShotgunBullet(AGameObject owner, Guid id, Vector2 direction)
			: base(owner, id, direction) 
		{
			Speed = Constants.SHOTGUN_BULLET_SPEED;
			Damage = Constants.SHOTGUN_BULLET_DAMAGE;
			HealthAmount = Constants.SHOTGUN_BULLET_LIFE_DISTANCE;
			ObjectType = EnumObjectType.ShutgunBullet;
		}
	}
}