using System;
using SkyShoot.Contracts;
using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon.Bullets
{
	public class PoisonTickBullet : AProjectile
	{
		public PoisonTickBullet(AGameObject owner, Guid id, Vector2 direction)
			: base(owner, id, direction)
		{
			Speed = Constants.POISONTICK_BULLET_SPEED;
			Damage = Constants.POISONTICK_BULLET_DAMAGE;
			HealthAmount = Constants.POISONTICK_BULLET_LIFE_DISTANCE;
			ObjectType = EnumObjectType.PoisonTickBullet;
		}
	}
}