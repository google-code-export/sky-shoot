using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts;
using SkyShoot.Service.Weapon;

// todo //!! 2 delete
namespace SkyShoot.ServProgram.Mobs
{
	class ShootingMobFactory: IMobFactory
	{
		private float _width;
		private float _height;
		private float _border;
		private float _health;
		private Random _random = new Random();

		public ShootingMobFactory(GameLevel gameLevel)
		{
			_width = gameLevel.levelWidth;
			_height = gameLevel.levelHeight;
			_border = Constants.LEVELBORDER;
			_health = 15;
		}

		public Mob CreateMob()
		{
			int x;
			int y;

			// присваивание случайных координат созданному мобу
			if (_random.Next(2) == 0) //длина
			{
				x = _random.Next(0, (int)(_width + _border * 2));

				if (_random.Next(2) == 0) // верх
				{
					y = _random.Next(0, (int)_border);
				}
				else //низ
				{
					y = _random.Next((int)(_height + _border), (int)(_height + _border * 2));
				}
			}
			else // высота
			{
				y = _random.Next(0, (int)(_height + _border * 2));

				if (_random.Next(2) == 0) // левая
				{
					x = _random.Next(0, (int)_border);
				}
				else // правая
				{
					x = _random.Next((int)(_width + _border), (int)(_width + _border * 2));
				}

			}
			var mob = new ShootingMob(_health, new Pistol(Guid.NewGuid()),5000);
			mob.Coordinates = new XNA.Framework.Vector2(x, y);
			_health *= 1.03f;
			return mob;
		}
	}
}
