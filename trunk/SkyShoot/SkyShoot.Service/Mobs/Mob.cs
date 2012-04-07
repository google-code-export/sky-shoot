using System;
using System.Collections.Generic;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Service;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.Mobs
{
	public class Mob : AGameObject
	{
		public AGameObject Target { get; set; }

		private int _thinkCounter;

		public Mob(float healthAmount)
		{
			ObjectType = EnumObjectType.Mob;
			_thinkCounter = 0;
			Id = Guid.NewGuid();
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

		public void FindTarget(List<AGameObject> targetPlayers)
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

		public override IEnumerable<AGameEvent> Think(List<AGameObject> players, long time)
		{
			var res = new AGameEvent[] { };
			if (_wait == 0)
			{
				if (_thinkCounter % 10 == 0)
				{
					if (!players.Contains(Target) || Target == null)
					{
						FindTarget(players);
					}
					if (Target == null)
						return res;
					RunVector = new Vector2(Target.Coordinates.X - Coordinates.X, Target.Coordinates.Y - Coordinates.Y);
				}
				_thinkCounter++;
			}
			else
			{
				_wait--;
			}
			return res;
		}

		private int _wait;

		private void Stop()
		{
			RunVector = new Vector2(0, 0);
			_wait = 30;
		}

		public override IEnumerable<AGameEvent> Do(AGameObject obj, long time)
		{
			// не кусаем друзей-товарищей
			//if(obj.Is(EnumObjectType.Mob))
			//  return;

			// ничего не кусаем окромя игроков злобных
			var player = obj as MainSkyShootService;
			if (_wait < 1 && obj.Is(EnumObjectType.Player) && (player != null))
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
