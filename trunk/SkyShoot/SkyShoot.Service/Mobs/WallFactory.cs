using System;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Session;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Mobs
{
	public class WallFactory
	{
		protected GameLevel GameLevel { get; set; }
		
		public WallFactory(GameLevel gameLevel)
		{
			GameLevel = gameLevel;
		}

		public Wall[] CreateWalls()
		{
			return new Wall[]
			{
				new Wall(new Vector2(GameLevel.levelWidth / 2, GameLevel.levelHeight / 2), 
				50f, Guid.NewGuid())
			};
		}

	}
}