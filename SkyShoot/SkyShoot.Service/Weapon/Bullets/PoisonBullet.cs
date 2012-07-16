using System;
using System.Collections.Generic;
using SkyShoot.Contracts;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.XNA.Framework;
using SkyShoot.ServProgram.Mobs;

namespace SkyShoot.Service.Weapon.Bullets
{
	class PoisonBullet : AProjectile
	{
		public PoisonBullet(AGameObject owner, Guid id, Vector2 direction)
			: base(owner, id, direction)
		{
			Speed = Constants.POISON_BULLET_SPEED;
			Damage = Constants.POISON_BULLET_DAMAGE;
			HealthAmount = Constants.POISON_BULLET_LIFE_DISTANCE;
			Radius = Constants.POISON_BULLET_RADIUS;
			ObjectType = EnumObjectType.PoisonBullet;
		}

		public override IEnumerable<AGameEvent> Do(AGameObject obj, List<AGameObject> newObjects, long time)
		{
			var res = new List<AGameEvent>(base.Do(obj, newObjects, time));
			var player = obj as MainSkyShootService;//Ради проверки на наличие зеркала
			if (player != null)
			{
				var mirror = player.GetBonus(EnumObjectType.Mirror);

				if (obj.Id != Owner.Id && obj.Is(EnumObjectType.Player) && (obj.HealthAmount >= Constants.POISON_BULLET_DAMAGE) && (mirror == null))
				{
					var wp = new PoisonTick(Guid.NewGuid());
					var Poison = new Poisoning(Constants.POISONING_TICK_TIMES, wp, obj);	//Время жизни--через здоровье
					Poison.ObjectType = EnumObjectType.Poisoning;
					Poison.Coordinates = obj.Coordinates;
					newObjects.Add(Poison);
				}
			}
			return res;
		}
	}
}
