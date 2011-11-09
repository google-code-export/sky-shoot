using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SkyShoot.Contracts.Session;

namespace SkyShoot.Contracts.Mobs
{
    public class SpiderFactory : IMobFactory
    {
        private Random _random = new Random();
        private Double _width;
        private Double _height;
        private Double _border;
        private int _healthAmount;
        private float _radius;
        private float _speed;


        public SpiderFactory(GameLevel gameLevel, int healthAmount, float radius, float speed)
        {
            _width = gameLevel.levelWidth;
            _height = gameLevel.levelHeight;
            _border = gameLevel.LEVELBORDER;
            _healthAmount = healthAmount;
            _speed = speed;
            _radius = radius;
        }

        public Mob CreateMob(List<AMob> targetPlayers)
        {
            Double x = _random.NextDouble() * (_border + _width);
            Double y;

            // присваивание случайных координат созданному мобу
            if (x <= _border || x >= _width)
            {
                y = _random.NextDouble() * (_border + _height);
            }
            else
            {
                if (_random.Next(1) == 0)
                {
                    y = _random.NextDouble() * _border;
                }
                else 
                {
                    y = _height - _random.NextDouble() * _border;
                }
            }

            var spider = new Mob();
            // поиск цели и логика атак/преследования потом

            spider.Coordinates = new Vector2((float) x, (float) y);
            spider.Radius = _radius;
            spider.Speed = _speed;
            spider.HealthAmount = _healthAmount;

            return spider;
        }
    }
}
