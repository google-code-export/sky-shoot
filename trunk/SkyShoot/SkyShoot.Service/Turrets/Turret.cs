using System.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SkyShoot.Contracts;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Session;
using SkyShoot.XNA.Framework;
using SkyShoot.ServProgram.Mobs;
using SkyShoot.Contracts.Weapon;

namespace SkyShoot.Service.Weapon.Bullets
{
	class Turret : ShootingMob
	{
		public Turret(float health, AWeapon weapon, int shootingDelay, AGameObject owner, Vector2 coordinates)
			: base(health, weapon, shootingDelay)
		{
			Weapon = weapon;
			Weapon.Owner = owner;
			Target = null;
			Coordinates = coordinates;
			ObjectType = EnumObjectType.Turret;
			Weapon = weapon;
			Weapon.Owner = owner;
			TeamIdentity = (owner.TeamIdentity);
			Radius = 10;
			Speed = 0f;
			ObjectType = EnumObjectType.LivingObject;
			ThinkCounter = 0;
			Id = Guid.NewGuid();
			MaxHealthAmount = HealthAmount = health;
			Damage = 20;
		}

		public override void FindTarget(List<AGameObject> targetObjects)
		{
			float distance = 1000000;

			foreach (var obj in targetObjects)
			{
				if (obj.Id == Weapon.Owner.Id || !obj.Is(EnumObjectType.LivingObject) || obj.Id == Id) //todo: teams
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
	}
}
