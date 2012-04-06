using System;
using SkyShoot.Contracts.Mobs;

using SkyShoot.Game.Client.GameObjects;

using SkyShoot.Game.Client.View;

namespace SkyShoot.Game.Client.Game
{
	internal class GameFactory
	{
		public static Mob CreateClientMob(AGameObject serverGameObject)
		{
			if (serverGameObject.Is(AGameObject.EnumObjectType.Player))
				return new Mob(serverGameObject, Textures.PlayerAnimation);

			if (serverGameObject.Is(AGameObject.EnumObjectType.Mob))
				return new Mob(serverGameObject, Textures.SpiderAnimation);

			throw new Exception();
		}

		public static Projectile CreateClientProjectile(AGameObject serverGameObject)
		{
			return new Projectile(serverGameObject);
		}

		public static GameBonus CreateClientGameBonus(AGameObject serverGameObject)
		{
			return new GameBonus(serverGameObject);
		}

		public static GameLevel CreateClientGameLevel(Contracts.Session.GameLevel gameLevel)
		{
			return new GameLevel(gameLevel);
		}
	}
}