using System;
using SkyShoot.Contracts;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon
{
	public class SpiderBullet : AProjectile
	{
		public SpiderBullet(AGameObject owner, Guid id, Vector2 direction)
			: base(owner, id, direction)
		{
			Speed = Constants.PISTOL_BULLET_SPEED;
			Damage = Constants.PISTOL_BULLET_DAMAGE;
			HealthAmount = Constants.PISTOL_BULLET_LIFE_DISTANCE;
			ObjectType = EnumObjectType.SpiderBullet;
		}
	}
}