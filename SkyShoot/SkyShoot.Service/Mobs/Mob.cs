using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SkyShoot.Contracts.Service;
using SkyShoot.Service;

namespace SkyShoot.Contracts.Mobs
{
    public class Mob : AMob
    {
        private const float _speed = 100; // заменить
        private const float _radius = 100; // заменить
        private static Double _health = 100; // заменить начальное здоровье

        public MainSkyShootService targetPlayer { get; set; }

        public Mob()
        {
            IsPlayer = false;
            Id = new Guid();
            HealthAmount = (int)_health;
            _health *= 1.001; // увеличение здоровья каждого следующего на 0.1%
            Speed = _speed;
            Radius = _radius;
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

        public event SomebodyMovesHadler MeMoved;

        public void Move() 
        {
            RunVector =new Vector2(targetPlayer.Coordinates.X - Coordinates.X, targetPlayer.Coordinates.Y - Coordinates.Y);
            RunVector.Normalize();
            ShootVector = RunVector;

            if (MeMoved != null)
            {
                MeMoved(this, RunVector);
            }
        }

        public void Think(long counter, List<MainSkyShootService> players)
        {
            if (counter % 6 == 0)// раз в 6 тиков(0.1 секунды)
            {
                if (!players.Contains(targetPlayer))
                {
                    FindTarget(players);
                }
                Move();
            }
        }
    }
}
