using System;

using SkyShoot.Contracts.Mobs;
using Microsoft.Xna.Framework;
using SkyShoot.Contracts.Weapon.Projectiles;
using SkyShoot.Contracts;

namespace SkyShoot.Service.Weapon.Bullets
{
	public class PistolBullet : AProjectile
	{
		
		private const EnumBulletType TYPE = EnumBulletType.Bullet;

		public PistolBullet(AMob owner, Guid id, Vector2 direction)
			: base(owner, id, direction) 
		{
			Speed = Constants.PISTOL_BULLET_SPEED;
			Damage = Constants.PISTOL_DAMAGE;
			LifeDistance = Constants.PISTOL_BULLET_LIFE_DISTANCE;
			Type = TYPE;
		} 
	}
}