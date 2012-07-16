using System;
using SkyShoot.Contracts;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.XNA.Framework;

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