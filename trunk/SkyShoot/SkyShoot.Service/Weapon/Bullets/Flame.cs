using System;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Service;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon.Bullets
{
	public class Flame : AProjectile
	{
		public Flame(AGameObject owner, Guid id, Vector2 direction, Vector2 coordinates)
			: base(owner, id, direction)
		{
			Coordinates = coordinates;
			Speed = Constants.FLAME_SPEED;
			Damage = Constants.FLAME_DAMAGE;
			HealthAmount = Constants.FLAME_LIFE_DISTANCE;
			Radius = Constants.FLAME_RADIUS;
			ObjectType = EnumObjectType.Flame;
		}
	}
}
