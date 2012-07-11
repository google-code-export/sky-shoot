using System;
using System.Collections.Generic;
using SkyShoot.Contracts;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;
using SkyShoot.ServProgram.Mobs;

namespace SkyShoot.Service.Weapon.Bullets
{
	class PoisonBullet : AProjectile
	{
		public PoisonBullet(AGameObject owner, Guid id, Vector2 direction)
			: base(owner, id, direction)
		{
			Speed = Constants.PISTOL_BULLET_SPEED;
			Damage = Constants.PISTOL_BULLET_DAMAGE;
			HealthAmount = Constants.PISTOL_BULLET_LIFE_DISTANCE;
			Radius = Constants.DEFAULT_BULLET_RADIUS;
			ObjectType = EnumObjectType.PoisonBullet;
		}

		public override IEnumerable<AGameEvent> Do(AGameObject obj, List<AGameObject> newObjects, long time)
		{
			var res = new List<AGameEvent>(base.Do(obj, newObjects, time));
			if (obj.Id != Owner.Id && obj.Is(EnumObjectType.Player) && (obj.HealthAmount >= Constants.PISTOL_BULLET_DAMAGE))
			{
				var wp = new PoisonTick(Guid.NewGuid());
				var Poison = new Poisoning(40f, wp, obj);	//Время жизни--через здоровье
				Poison.ObjectType = EnumObjectType.Poisoning;
				Poison.Coordinates.X = obj.Coordinates.X;
				Poison.Coordinates.Y = obj.Coordinates.Y;
				newObjects.Add(Poison);
			}

			return res;
		}
	}
}
