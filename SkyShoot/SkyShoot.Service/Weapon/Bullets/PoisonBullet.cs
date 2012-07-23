using System;
using System.Collections.Generic;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Service;
using SkyShoot.ServProgram.Mobs;
using SkyShoot.XNA.Framework;

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
			
			
			Bonuses.AGameBonus mirror = null;
			if (obj.Is(EnumObjectType.Player))
			{
				var afflicted = obj as MainSkyShootService; //Ради проверки на наличие зеркала
				if (afflicted != null) mirror = afflicted.GetBonus(EnumObjectType.Mirror);
			}

			if (obj.Is(EnumObjectType.LivingObject) && (obj.HealthAmount >= Constants.POISON_BULLET_DAMAGE) &&
				(mirror == null) && (obj.TeamIdentity != Owner.TeamIdentity) && !obj.Is(EnumObjectType.Turret))
			{
				var wp = new Pistol(Guid.NewGuid());//Просто для конструктора. Заглушка.
				var poison = new Poisoning(Constants.POISONING_TICK_TIMES, wp, obj)
								{
									ObjectType = EnumObjectType.Poisoning,
								}; //Время жизни--через здоровье
				newObjects.Add(poison);
			}
			
			return res;
		}
	}
}
