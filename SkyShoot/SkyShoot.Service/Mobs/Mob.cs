using System;
using System.Collections.Generic;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Service;
using SkyShoot.XNA.Framework;

namespace SkyShoot.ServProgram.Mobs
{
	public class Mob : AGameObject
	{
		public AGameObject Target { get; set; }
		// todo //!! convert this to time from the update counnter
		protected int ThinkCounter;
		protected int Wait;

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
				if (!pl.Is(EnumObjectType.Player))
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
		}

		public override IEnumerable<AGameEvent> Think(List<AGameObject> gameObjects, List<AGameObject> newGameObjects, long time)
		{
			var res = new AGameEvent[] { };
			if (Wait == 0)
			{
				if (ThinkCounter % 10 == 0)
				{
					if (!gameObjects.Contains(Target) || Target == null)
					{
						FindTarget(gameObjects);
					}
					if (Target == null)
						return res;
					RunVector = new Vector2(Target.Coordinates.X - Coordinates.X, Target.Coordinates.Y - Coordinates.Y);
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
			RunVector = new Vector2(0, 0);
			Wait = 30;
		}

		public override IEnumerable<AGameEvent> Do(AGameObject obj, List<AGameObject> newObjects, long time)
		{
			// не кусаем друзей-товарищей
			//if(obj.Is(EnumObjectType.Mob))
			//  return;

			// ничего не кусаем окромя игроков злобных
			var player = obj as MainSkyShootService;
			if (Wait < 1 && obj.Is(EnumObjectType.Player) && (player != null))
			{
				var shield = player.GetBonus(EnumObjectType.Shield);
				var damage = shield == null ? 1f : shield.DamageFactor;
				player.HealthAmount -= damage * Damage;
				Stop();
				return new AGameEvent[]
								{
									new ObjectHealthChanged(player.HealthAmount, player.Id, time), 
									new ObjectDirectionChanged(RunVector, Id, time)
								};
			}
			return new AGameEvent[] { };
		}
	}
}
