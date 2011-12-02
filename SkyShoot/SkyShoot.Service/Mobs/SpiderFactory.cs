using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SkyShoot.Contracts.Session;
using SkyShoot.Service.Mobs;
using SkyShoot.Service.Weapon;

namespace SkyShoot.Contracts.Mobs
{
    public class SpiderFactory : IMobFactory
    {
        private float _width;
        private float _height;
        private float _border;
		private float _health; 


        public SpiderFactory(GameLevel gameLevel)
        {
            _width = gameLevel.levelWidth;
            _height =  gameLevel.levelHeight;
            _border = Constants.LEVELBORDER;
			_health = 100; //change to real value
        }

        private Random _random = new Random();

        public Mob CreateMob()
        {
            int x;
            int y;

            // присваивание случайных координат созданному мобу
            if (_random.Next(2) == 0) //длина
            {
                x = _random.Next( 0, (int) (_width + _border * 2));

                if (_random.Next(2) == 0) // верх
                {
                    y = _random.Next( 0, (int) _border);
                }
                else //низ
                {
                    y = _random.Next((int) (_height + _border), (int) (_height + _border * 2));
                }
            }
            else // высота
            {
                y = _random.Next( 0, (int) (_height + _border *2));

                if (_random.Next(2) == 0) // левая
                {
                    x = _random.Next( 0, (int) _border);
                }
                else // правая
                {
                    x = _random.Next((int) (_width + _border), (int) (_width + _border*2));
                }

            }

            var spider = new Spider((float)_health);
			_health *= 1.001f;
            spider.Coordinates = new Vector2((float) x, (float) y);
			spider.Weapon = new Claw(Guid.NewGuid(),spider);
            return spider;
        }
    }
}
