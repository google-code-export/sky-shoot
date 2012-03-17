using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.XNA.Framework;
using SkyShoot.Contracts.Service;
using SkyShoot.Service;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.Contracts.Weapon.Projectiles;

namespace SkyShoot.Contracts.Mobs
{
	public class Mob : AGameObject
	{
		public MainSkyShootService targetPlayer { get; set; }

		public float Damage { get; set; }

		private int _counter;

		public Mob(float healthAmount)
		{
			IsPlayer = false;
			_counter = 0;
			Id = Guid.NewGuid();
			MaxHealthAmount = HealthAmount = healthAmount;
			Damage = 10;
		}

		public void FindTarget(List<MainSkyShootService> targetPlayers)
		{
			float distance = 1000000;
			float temp;

			for (int i = 0; i < targetPlayers.Count;i++ )
			{
				temp = Vector2.Distance(Coordinates, targetPlayers[i].Coordinates);

				if (distance > temp)
				{
					distance = temp;
					targetPlayer = targetPlayers[i];
				}
			}
		}

		//public event SomebodyMovesHandler MeMoved;

		public virtual void Move()
		{
			if(targetPlayer == null)
				return;
			RunVector = new Vector2(targetPlayer.Coordinates.X - Coordinates.X, targetPlayer.Coordinates.Y - Coordinates.Y);
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

		public virtual void Think(List<MainSkyShootService> players)
		{
			if (wait == 0)
			{
				if (_counter % 10 == 0)
				{
					if (!players.Contains(targetPlayer) || targetPlayer == null)
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
