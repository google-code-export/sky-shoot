using System;
using System.Linq;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;
using SkyShoot.XNA.Framework;

namespace SkyShoot.ServProgram.Mobs
{
	public class DeathmatchSpiderFactory : SpiderFactory
	{
		private readonly float _width;
		private readonly float _height;
		private readonly float _border;
		private readonly Random _random;
		private float _health;
		private readonly int[] _allowedMobs;
		private int _simpleMobsWasBorn; //чтобы разбавлять обычных мобов мощными

		public DeathmatchSpiderFactory(GameLevel gameLevel)
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
				(int)MobType.SpiderWithSimpleMind,
				(int)MobType.Caterpillar
			};
			_simpleMobsWasBorn = 0;
		}

		public override Mob CreateMob()
		{
			int nextMob;
			do
			{
				nextMob = _random.Next(360)%_allowedMobs.Count();
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

		/// <summary>
		/// присваивание случайных координат созданному мобу
		/// </summary>
		protected override Vector2 GetRandomCoord()
		{
			return new Vector2(500 + _random.Next(-150, 150), 500 + _random.Next(-150, 150));//Чтобы игроки бегали по периферии, а не устраивали арену в центре.
		}
	}
}
