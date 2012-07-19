using System;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;
using SkyShoot.Service.Mobs;
using SkyShoot.Service.Weapon;
using SkyShoot.XNA.Framework;

namespace SkyShoot.ServProgram.Mobs
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
			_width = gameLevel.LevelWidth;
			_height = gameLevel.LevelHeight;
			_border = Constants.LEVELBORDER;
			_health = 10; //change to real value
		}

		public Mob CreateMob()
		{
			Mob spider;
			switch (_random.Next(100) % 10)//Обычные пауки генерируются чаще остальных.
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
					spider = new ParentMob();
					break;
				case 4:
					spider = new Hydra();
					break;
				case 5:
					var wp = new PoisonGun(Guid.NewGuid());
					spider = new Poisoner(Constants.POISONER_MOB_HEALTH, wp, 1000);
					break;
				default:
					spider = new Spider(_health);
					break;
			}

			spider.TeamIdentity = null;//Мобам зануляем
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

				y = _random.Next(2) == 0
						? _random.Next(0, (int)_border)
						: _random.Next((int)(_height + _border), (int)(_height + _border * 2));
			}
			else // высота
			{
				y = _random.Next(0, (int)(_height + _border * 2));

				x = _random.Next(2) == 0
						? _random.Next(0, (int)_border)
						: _random.Next((int)(_width + _border), (int)(_width + _border * 2));
			}

			return new Vector2(x, y);
		}
	}
}
