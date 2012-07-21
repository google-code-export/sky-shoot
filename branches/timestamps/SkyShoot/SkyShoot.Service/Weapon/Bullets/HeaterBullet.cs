using System;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Service;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon.Bullets
{
	public class HeaterBullet : AProjectile
	{
		public HeaterBullet(AGameObject owner, Guid id, Vector2 direction)
			: base(owner, id, direction)
		{
			Speed = Constants.HEATER_BULLET_SPEED;
			Damage = Constants.HEATER_BULLET_DAMAGE;
			HealthAmount = Constants.HEATER_BULLET_LIFE_DISTANCE;
			//!! 2 do сделать отдельный пункт енума
			ObjectType = EnumObjectType.HeaterBullet;
		}
	}
}