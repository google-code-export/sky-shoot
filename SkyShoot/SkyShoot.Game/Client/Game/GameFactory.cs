using System;
using SkyShoot.Contracts.Mobs;

using SkyShoot.Game.Client.GameObjects;

using SkyShoot.Game.Client.View;

namespace SkyShoot.Game.Client.Game
{
	internal class GameFactory
	{
		public static DrawableGameObject CreateClientMob(AGameObject serverGameObject)
		{
			// can't uhse switch 'cause one object can have many merged EnumTypes
			if (serverGameObject.Is(AGameObject.EnumObjectType.Player))
				return new DrawableGameObject(serverGameObject, Textures.PlayerAnimation);

			if (serverGameObject.Is(AGameObject.EnumObjectType.Mob))
				return new DrawableGameObject(serverGameObject, Textures.SpiderAnimation);

			if (serverGameObject.Is(AGameObject.EnumObjectType.Bullet))
				return new DrawableGameObject(serverGameObject, Textures.ProjectileTexture);
			
			if (serverGameObject.Is(AGameObject.EnumObjectType.Bonus))
				return new DrawableGameObject(serverGameObject, Textures.Plus);

			if(serverGameObject.Is(AGameObject.EnumObjectType.Wall))
				return new DrawableGameObject(serverGameObject, Textures.OneStone);

			throw new Exception();
		}
		
		//public static Projectile CreateClientProjectile(AGameObject serverGameObject)
		//{
		//  return new Projectile(serverGameObject);
		//}

		//public static GameBonus CreateClientGameBonus(AGameObject serverGameObject)
		//{
		//  return new GameBonus(serverGameObject);
		//}

		public static GameLevel CreateClientGameLevel(Contracts.Session.GameLevel gameLevel)
		{
			return new GameLevel(gameLevel);
		}
	}
}