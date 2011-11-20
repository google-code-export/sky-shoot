using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SkyShoot.Contracts.Session;
using SkyShoot.Service.Mobs;

namespace SkyShoot.Contracts.Mobs
{
    public class SpiderFactory : IMobFactory
    {
        private float _width;
        private float _height;
        private float _border;


        public SpiderFactory(GameLevel gameLevel)
        {
            _width = gameLevel.levelWidth;
            _height =  gameLevel.levelHeight;
            _border = gameLevel.LEVELBORDER;
        }

        private Random _random = new Random();

        public Mob CreateMob()
        {
            int x;
            int y;

            // присваивание случайных координат созданному мобу
            if (_random.Next(2) == 0) //длина
            {
                x = _random.Next((int) (-_width - _border), (int) (_width + _border));

                if (_random.Next(2) == 0) // верх
                {
                    y = _random.Next((int) _height, (int) (_height + _border));
                }
                else //низ
                {
                    y = _random.Next((int)(-_height - _border), (int) -_height);
                }
            }
            else // высота
            {
                y = _random.Next((int) (-_height - _border), (int)(_height + _border));

                if (_random.Next(2) == 0) // левая
                {
                    x = _random.Next((int) (-_width - _border), (int) -_width);
                }
                else // правая
                {
                    x = _random.Next((int) _width, (int) (_width + _border));
                }

            }

            var spider = new Spider();
            spider.Coordinates = new Vector2((float) x, (float ) y);
            return spider;
        }
    }
}
