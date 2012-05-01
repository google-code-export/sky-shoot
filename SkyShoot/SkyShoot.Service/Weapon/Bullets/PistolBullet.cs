using System;

using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;
using SkyShoot.Contracts;

namespace SkyShoot.Service.Weapon.Bullets
{
	public class PistolBullet : AProjectile
	{
		public PistolBullet(AGameObject owner, Guid id, Vector2 direction)
			: base(owner, id, direction) 
		{
			Speed = Constants.PISTOL_BULLET_SPEED;
			Damage = Constants.PISTOL_BULLET_DAMAGE;
			HealthAmount = Constants.PISTOL_BULLET_LIFE_DISTANCE;
			ObjectType = EnumObjectType.PistolBullet;
		} 
	}
}