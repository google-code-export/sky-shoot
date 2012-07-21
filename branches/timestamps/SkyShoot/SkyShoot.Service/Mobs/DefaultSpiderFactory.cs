using System;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;
using SkyShoot.Service.Mobs;
using SkyShoot.Service.Weapon;
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

		public DefaultSpiderFactory(GameLevel gameLevel)
		{
			_random = new Random();
			_width = gameLevel.Width;
			_height = gameLevel.Height;
			_border = Constants.LEVELBORDER;
			_health = 10; //change to real value
		}

		public override Mob CreateMob()
		{
			return base.CreateMob(_random.Next(100) % 10);//Простые паучки генерируются чаще других.
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
