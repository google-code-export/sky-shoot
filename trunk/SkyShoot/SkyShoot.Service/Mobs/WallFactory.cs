using System;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Session;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Mobs
{
	public class WallFactory
	{
		public const int StonesNumber = 20;
		protected GameLevel GameLevel { get; set; }
		
		public WallFactory(GameLevel gameLevel)
		{
			GameLevel = gameLevel;
		}

		public Wall[] CreateWalls()
		{
			var res = new Wall[StonesNumber];
			var random = new Random();
			for (var i = 0; i < StonesNumber; i++)
			{
				res[i] = new Wall(new Vector2(random.Next((int)GameLevel.levelWidth),
					random.Next((int)GameLevel.levelHeight)),
				                 random.Next(10, 30), Guid.NewGuid());
			}
			//return res;
			return new Wall[]
			{
				new Wall(new Vector2(-200+GameLevel.levelWidth / 2, -200+GameLevel.levelHeight / 2), 
				50f, Guid.NewGuid())
			};
		}
	}
}