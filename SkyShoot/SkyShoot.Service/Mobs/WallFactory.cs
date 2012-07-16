using System;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Session;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Mobs
{
	public class WallFactory
	{
		public const int STONES_NUMBER = 20;

		private readonly float _levelWidth;
		private readonly float _levelHeight;

		public WallFactory(GameLevel gameLevel)
		{
			_levelHeight = gameLevel.LevelHeight;
			_levelWidth = gameLevel.LevelWidth;
		}

		public Wall[] CreateWalls()
		{
			var res = new Wall[STONES_NUMBER];
			var random = new Random();
			for (var i = 0; i < STONES_NUMBER; i++)
			{
				res[i] = new Wall(
					new Vector2(random.Next((int)_levelWidth), random.Next((int)_levelHeight)),
					random.Next(10, 30),
					Guid.NewGuid());
			}
			return res;
		}
	}
}