using System;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;
using SkyShoot.Service.Mobs;
using SkyShoot.Service.Weapon;
using SkyShoot.XNA.Framework;

namespace SkyShoot.ServProgram.Mobs
{
	public enum MobType: int
	{
		Spider = 0,
		ShootingMob = 1,
		ParentMob = 2,
		Hydra = 3,
		Poisoner = 4,
		SpiderWithSimpleMind = 5
	};

	public class SpiderFactory : IMobFactory
	{
		private readonly float _width;
		private readonly float _height;
		private readonly float _border;
		private readonly Random _random;
		private float _health;

		public SpiderFactory() 	
		{
			_random = new Random();
			_border = Constants.LEVELBORDER;
			_health = 10; //change to real value
		}

		public SpiderFactory(GameLevel gameLevel)
		{
			_random = new Random();
			_width = gameLevel.LevelWidth;
			_height = gameLevel.LevelHeight;
			_border = Constants.LEVELBORDER;
			_health = 10; //change to real value
		}

		public virtual Mob CreateMob() { Mob spider = new SpiderWithSimpleMind(Constants.CHILDREN_MOB_HEALTH); return spider; }

		public Mob CreateMob(int newMob)
		{
			Mob spider;
			switch (newMob)//Обычные пауки генерируются чаще остальных.
			{
				case 0:
					spider = new Spider(_health);
					break;
				case 1:
					var w = new SpiderWeapon(Guid.NewGuid());
					spider = new ShootingMob(_health, w, 1000);
					break;
				case 2:
					spider = new ParentMob();
					break;
				case 3:
					spider = new Hydra();
					break;
				case 4:
					var wp = new PoisonGun(Guid.NewGuid());
					spider = new Poisoner(Constants.POISONER_MOB_HEALTH, wp, 1000);
					break;
				case 5:
				default:
					spider = new SpiderWithSimpleMind(_health);
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
		protected virtual Vector2 GetRandomCoord()
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
