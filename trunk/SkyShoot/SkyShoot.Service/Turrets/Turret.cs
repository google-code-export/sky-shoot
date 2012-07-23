using System;
using System.Collections.Generic;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Weapon;
using SkyShoot.ServProgram.Mobs;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon.Bullets
{
	class Turret : ShootingMob
	{
		public AGameObject Owner;
		private int lifeTime;

		public override bool IsActive
		{
			get { return base.IsActive && lifeTime > 0; }
		}

		public Turret(float health, AWeapon weapon, int shootingDelay, AGameObject owner, Vector2 coordinates)
			: base(health, weapon, shootingDelay)
		{
			lifeTime = Constants.TURRET_LIFETIME;
			Weapon = weapon;
			Weapon.Owner = owner;
			Owner = owner;
			Target = null;
			Coordinates = coordinates;
			ObjectType = EnumObjectType.Turret;
			TeamIdentity = (owner.TeamIdentity);
			Radius = Constants.TURRET_RADIUS;
;
			Speed = 0f;
			ThinkCounter = 0;
			Id = Guid.NewGuid();
			MaxHealthAmount = HealthAmount = health;
			Damage = 20;
		}

		public override void FindTarget(List<AGameObject> targetObjects)
		{
			float distance = Constants.TURRET_TARGET_SEARCHING_RANGE;

			foreach (var obj in targetObjects)
			{
				if (obj.TeamIdentity == TeamIdentity ||
					!obj.Is(EnumObjectType.LivingObject) ||
					obj.Is(EnumObjectType.Poisoning)
					)
				{
					continue;
				}
				float temp = Vector2.Distance(Coordinates, obj.Coordinates);

				if (distance > temp)
				{
					distance = temp;
					Target = obj;
				}
			}
		}

		public override IEnumerable<AGameEvent> Do(AGameObject obj, List<AGameObject> newObjects, long time)
		{
			return new AGameEvent[] { };
		}

		public override IEnumerable<AGameEvent> Think(List<AGameObject> gameObjects, List<AGameObject> newGameObjects, long time)
		{
			lifeTime--;
			if (Target != null && (!Target.IsActive ||
									Vector2.Distance(Coordinates, Target.Coordinates) > Constants.TURRET_TARGET_SEARCHING_RANGE))
			{
				Target = null;
			}
			return base.Think(gameObjects, newGameObjects, time);
		}
	}
}
