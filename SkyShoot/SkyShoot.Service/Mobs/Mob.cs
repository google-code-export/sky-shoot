using System;

using System.Collections.Generic;

using SkyShoot.XNA.Framework;

using SkyShoot.Service;

using SkyShoot.Contracts.Weapon.Projectiles;

namespace SkyShoot.Contracts.Mobs
{
	public class Mob : AGameObject
	{
		public MainSkyShootService TargetPlayer { get; set; }

		// already inherited
		// public float Damage { get; set; }

		private int _counter;

		public Mob(float healthAmount)
		{
			Type = EnumObjectType.Mob;
			_counter = 0;
			Id = Guid.NewGuid();
			MaxHealthAmount = HealthAmount = healthAmount;
			Damage = 10;
		}

		public void FindTarget(List<AGameObject> targetPlayers)
		{
			float distance = 1000000;
			float temp;

			for (int i = 0; i < targetPlayers.Count;i++ )
			{
				if ((targetPlayers[i]).GetType() != typeof(MainSkyShootService))
				{
					continue;
				}
				temp = Vector2.Distance(Coordinates, targetPlayers[i].Coordinates);

				if (distance > temp)
				{
					distance = temp;
					TargetPlayer = (MainSkyShootService)targetPlayers[i];
				}
			}
		}

		//public event SomebodyMovesHandler MeMoved;

		protected virtual void Move()
		{
			if(TargetPlayer == null)
				return;
			RunVector = new Vector2(TargetPlayer.Coordinates.X - Coordinates.X, TargetPlayer.Coordinates.Y - Coordinates.Y);
			RunVector = Vector2.Normalize(RunVector);
			ShootVector = RunVector;

			/*
			if (MeMoved != null)
			{
				System.Diagnostics.Trace.WriteLine("Mob run vector" + RunVector);
				MeMoved(this, RunVector);
			}
			*/
		}

		public override void  Think(List<AGameObject> players = null)
		{
			if (wait == 0)
			{
				if (_counter % 10 == 0)
				{
					if (!players.Contains(TargetPlayer) || TargetPlayer == null)
					{
						FindTarget(players);
					}
					Move();
				}
				_counter++;
			}
			else
			{
				wait--;
			}

			Move();
		}

		private int wait;

		public void Stop()
		{
			RunVector = new Vector2(0, 0);
			wait = 30;
		}

		public void DamageTaken(AProjectile bullet)
		{
			HealthAmount -= bullet.Damage;
		}
	}
}
