using System;
using System.Collections.Generic;
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
			Damage = 10;
		}

		public void FindTarget(List<AGameObject> targetPlayers)
		{
			float distance = 1000000;

			for (int i = 0; i < targetPlayers.Count;i++ )
			{
				var pl = targetPlayers[i];
				if(!pl.IsPlayer)
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

		//public override void Move()
		//{

		//  //base.Move();
		//}

		public override void  Think(List<AGameObject> players, long time)
		{
			if (_wait == 0)
			{
				if (_thinkCounter % 10 == 0)
				{
					if (!players.Contains(Target) || Target == null)
					{
						FindTarget(players);
					}
					if (Target == null)
						return;
					RunVector = new Vector2(Target.Coordinates.X - Coordinates.X, Target.Coordinates.Y - Coordinates.Y);
				}
				_thinkCounter++;
			}
			else
			{
				_wait--;
			}
		}

		private int _wait;

		private void Stop()
		{
			RunVector = new Vector2(0, 0);
			_wait = 30;
		}

		public override void Do(AGameObject obj, long time)
		{
			// не кусаем друзей-товарищей
			if(obj.Is(EnumObjectType.Mob))
				return;
			obj.HealthAmount -= Damage;
			Stop();
		}

		//public void DamageTaken(AProjectile bullet)
		//{
		//  HealthAmount -= bullet.Damage;
		//}
	}
}
