using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SkyShoot.Contracts.Service;
using SkyShoot.Service;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.Contracts.Weapon.Projectiles;

namespace SkyShoot.Contracts.Mobs
{
    public class Mob : AMob
    {
        private static Double _health = 100; //change to real value 


        public MainSkyShootService targetPlayer { get; set; }

        public float Damage { get; set; }

        private int _counter;

        public Mob()
        {
            IsPlayer = false;
            _counter = 0;
            Id = Guid.NewGuid();
            HealthAmount = (float)_health;
            Damage = 10;
            _health *= 1.001; // увеличение здоровья каждого следующего на 0.1%
        }

        public void FindTarget(List<MainSkyShootService> targetPlayers) 
        {
            float distance = 1000000;
            float temp;

            foreach (MainSkyShootService player in targetPlayers)
            {
                temp = Vector2.Distance(Coordinates, player.Coordinates);

                if (distance > temp)
                {
                    distance = temp;
                    targetPlayer = player;
                }
            }
        }

        public event SomebodyMovesHandler MeMoved;

				public virtual void Move()
				{
					RunVector = new Vector2(targetPlayer.Coordinates.X - Coordinates.X, targetPlayer.Coordinates.Y - Coordinates.Y);
                    RunVector = Vector2.Normalize(RunVector);
					ShootVector = RunVector;

					if (MeMoved != null)
					{
						System.Diagnostics.Trace.WriteLine("Mob run vector" + RunVector);
						MeMoved(this, RunVector);
					}
				}

        public virtual void Think(List<MainSkyShootService> players)
        {
            if (_counter % 20 == 0)
            {
                if (!players.Contains(targetPlayer) || targetPlayer == null)
                {
                    FindTarget(players);
                }
                Move();
            }
            _counter++;
        }

        public void DemageTaken(AProjectile bullet)
        {
            HealthAmount -= bullet.Damage;
        }
    }
}
