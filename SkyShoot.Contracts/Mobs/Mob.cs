using System;
using System.Collections.Generic;
using SkyShoot.XNA.Framework;
using SkyShoot.Contracts.Weapon.Projectiles;

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
				float temp = Vector2.Distance(Coordinates, targetPlayers[i].Coordinates);

				if (distance > temp)
				{
					distance = temp;
					Target = targetPlayers[i];
				}
			}
		}

		//public event SomebodyMovesHandler MeMoved;

		public override void Move()
		{
			if (Target == null)
				return;
			RunVector = new Vector2(Target.Coordinates.X - Coordinates.X, Target.Coordinates.Y - Coordinates.Y);

			base.Move();
		}

		public override void  Think(List<AGameObject> players)
		{
			if (_wait == 0)
			{
				if (_thinkCounter % 10 == 0)
				{
					if (!players.Contains(Target) || Target == null)
					{
						FindTarget(players);
					}
					Move();
				}
				_thinkCounter++;
			}
			else
			{
				_wait--;
			}
		}

		private int _wait;

		public void Stop()
		{
			RunVector = new Vector2(0, 0);
			_wait = 30;
		}

		public void DamageTaken(AProjectile bullet)
		{
			HealthAmount -= bullet.Damage;
		}
	}
}
