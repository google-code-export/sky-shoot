using System;
using SkyShoot.Contracts.Session;
using SkyShoot.Service.Mobs;
using SkyShoot.Service.Weapon;
using SkyShoot.ServProgram.Mobs;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.Mobs
{
	public class SpiderFactory : IMobFactory
	{
		private readonly float _width;
		private readonly float _height;
		private readonly float _border;
		private readonly Random _random;
		private float _health;

		public SpiderFactory(GameLevel gameLevel)
		{
			_random = new Random();
			_width = gameLevel.levelWidth;
			_height = gameLevel.levelHeight;
			_border = Constants.LEVELBORDER;
			_health = 10; //change to real value
		}

		public Mob CreateMob()
		{
			Mob spider;
			switch (_random.Next(4))
			{
				case 0:
					spider = new SpiderWithSimpleMind(_health);
					break;
				case 1:
					spider = new SpiderWithSimpleMind(_health);
					break;
				case 2:
					var w = new SpiderWeapon(Guid.NewGuid());
					spider = new ShootingMob(_health, w, 1000);
					break;
				case 3:
					spider = new ParentMob(Constants.PARENT_MOB_HEALTH);
					break;
				default:
					spider = new Spider(_health);
					break;
			}

			_health *= 1.05f;
			spider.Coordinates = GetRandomCoord();
			//spider.Weapon = new Claw(Guid.NewGuid(), spider);
			return spider;
		}

		/// <summary>
		/// присваивание случайных координат созданному мобу
		/// </summary>
		private Vector2 GetRandomCoord()
		{
			int x;
			int y;
			if (_random.Next(2) == 0) // длина
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

			return new Vector2(x, y);
		}
	}
}
