using System;
using System.Linq;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;
using SkyShoot.XNA.Framework;

namespace SkyShoot.ServProgram.Mobs
{
	public class DefaultSpiderFactory : SpiderFactory
	{
		private readonly float _width;
		private readonly float _height;
		private readonly float _border;
		private readonly Random _random;
		private float _health;
		private readonly int[] _allowedMobs;
		private int _simpleMobsWasBorn; //чтобы разбавлять обычных мобов мощными

		public DefaultSpiderFactory(GameLevel gameLevel)
		{
			_random = new Random();
			_width = gameLevel.Width;
			_height = gameLevel.Height;
			_border = Constants.LEVELBORDER;
			_health = 10; //change to real value

			_allowedMobs = new[] 
			{ 
				(int)MobType.Spider,
				(int)MobType.ShootingMob,
				(int)MobType.ParentMob,
				(int)MobType.Hydra,
				(int)MobType.Poisoner,
				(int)MobType.Caterpillar
			};
			_simpleMobsWasBorn = 0;
		}

		public override Mob CreateMob()
		{
			int nextMob;
			do
			{
				nextMob = _random.Next(360) % _allowedMobs.Count();
				if (nextMob == (int)MobType.ParentMob)
				{
					if (_simpleMobsWasBorn >= Constants.PARENT_MOB_RESPAWN_PER_SIMPLE)
					{
						_simpleMobsWasBorn -= Constants.PARENT_MOB_RESPAWN_PER_SIMPLE;
					}
					else
					{
						nextMob = 100;//Можно что поэлегантнее придумать. Пока так.
					}
				}
				else
				{
					if (_simpleMobsWasBorn < 250)
					{
						_simpleMobsWasBorn++;
					}
				}
			}
			while (!_allowedMobs.Contains(nextMob));//Генерирует следующее число, пока не попадёт в одно из массива.

			return CreateMob(nextMob);//Кидаем в наш виртуальный класс.
		}

		protected override Vector2 GetRandomCoord()
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
