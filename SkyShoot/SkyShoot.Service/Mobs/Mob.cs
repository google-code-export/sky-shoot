﻿using System;
using System.Collections.Generic;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Service;
using SkyShoot.XNA.Framework;
using SkyShoot.Contracts.Service;

namespace SkyShoot.ServProgram.Mobs
{
	public class Mob : AGameObject
	{
		public AGameObject Target { get; set; }
		// todo //!! convert this to time from the update counnter
		protected int ThinkCounter;
		protected int Wait;
		protected float DefaultSpeed;

		public Mob(float healthAmount)
			: base(Vector2.Zero, Guid.NewGuid())
		{
			ObjectType = EnumObjectType.Mob;
			ThinkCounter = 0;
			//Id = Guid.NewGuid();
			MaxHealthAmount = HealthAmount = healthAmount;
			Damage = 20;
		}

		public override void Copy(AGameObject other)
		{
			base.Copy(other);
			var m = other as Mob;
			if (m == null)
				return; //!! throw
			Target = m.Target;
		}

		public virtual void FindTarget(List<AGameObject> targetPlayers)
		{
			float distance = 1000000;

			foreach (var pl in targetPlayers)
			{
				if (!pl.Is(EnumObjectType.LivingObject) || pl.Is(EnumObjectType.Poisoning) || (pl.TeamIdentity == TeamIdentity))
				{
					continue;
				}
				float temp = Vector2.Distance(Coordinates, pl.Coordinates);

				if (distance > temp)
				{
					distance = temp;
					Target = pl;
				}
			}
			//if ((Target == null) || (Target.IsActive))
			//{
			//    RunVector = new Vector2(500f - Coordinates.X, 500f - Coordinates.Y);
			//}
		}

		public override IEnumerable<AGameEvent> Think(List<AGameObject> gameObjects, List<AGameObject> newGameObjects, long time)
		{
			var res = new AGameEvent[] { };
			if (Wait == 0)
			{
				Speed = DefaultSpeed;
				if (ThinkCounter % 10 == 0)
				{
					if (!gameObjects.Contains(Target) || Target == null)
					{
						FindTarget(gameObjects);
					}
					if (Target == null)
						return res;
					Vector2 DirectionOnTarget = new Vector2(Target.Coordinates.X - Coordinates.X, Target.Coordinates.Y - Coordinates.Y);
					if (DirectionOnTarget.LengthSquared() > Constants.EPSILON)
						RunVector = DirectionOnTarget;
				}
				ThinkCounter++;
			}
			else
			{
				Wait--;
			}
			ShootVector = RunVector;

			return res;
		}


		private void Stop()
		{
			Speed = 0;
			Wait = 30;
		}

		public override IEnumerable<AGameEvent> Do(AGameObject obj, List<AGameObject> newObjects, long time)
		{
			// не кусаем друзей-товарищей
			//if(obj.Is(EnumObjectType.Mob))
			//  return;

			if (Wait < 1 && TeamIdentity != obj.TeamIdentity && obj != null && obj.Is(EnumObjectType.LivingObject)) //Кусать живые обьекты не из своей команды
			{
				if (obj.Is(EnumObjectType.Player))
				{
					var player = obj as MainSkyShootService;
					var shield = player.GetBonus(EnumObjectType.Shield);
					var damage = shield == null ? 1f : shield.DamageFactor;
					obj.HealthAmount -= damage * Damage;
					if (TeamIdentity != null)
					{
						int teamMembers = TeamIdentity.Members.Count;
						foreach (AGameObject member in TeamIdentity.Members)
						{
							var _player = member as MainSkyShootService;
							if (_player != null) _player.Tracker.AddExpTeam(_player, obj, (int)(damage * Damage), teamMembers);
						}
					}
				}
				else
				{
					obj.HealthAmount -= Damage;
					if (TeamIdentity != null)
					{
						int teamMembers = TeamIdentity.Members.Count;
						foreach (AGameObject member in TeamIdentity.Members)
						{
							var _player = member as MainSkyShootService;
							if (_player != null) _player.Tracker.AddExpTeam(_player, obj, (int)(Damage), teamMembers);
						}
					}
				}
				Stop();
				return new AGameEvent[]
								{
									new ObjectHealthChanged(obj.HealthAmount, obj.Id, time), 
									new ObjectDirectionChanged(RunVector, Id, time)
								};
			}
			return new AGameEvent[] { };
		}
	}
}
