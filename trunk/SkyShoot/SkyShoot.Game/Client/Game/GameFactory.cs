using System;

using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Game.Client.GameObjects;
using SkyShoot.Game.Client.View;

namespace SkyShoot.Game.Client.Game
{
	internal class GameFactory
	{
		public static Mob CreateClientMob(AGameObject mob)
		{
			// todo mob type
			switch (mob.Type)
			{
				case AGameObject.EnumObjectType.Player:
					return new Mob(mob, Textures.PlayerAnimation);
				case AGameObject.EnumObjectType.Mob:
					return new Mob(mob, Textures.SpiderAnimation);
			}
			return null;
		}

		public static GameLevel CreateClientGameLevel(Contracts.Session.GameLevel gameLevel)
		{
			return new GameLevel(gameLevel);
		}

		public static ABonus CreateClientBonus(AObtainableDamageModifier bonus)
		{
			throw new NotImplementedException();
		}

		//!!  @todo delete this
		public static Projectile CreateClientProjectile(Contracts.Weapon.Projectiles.AProjectile projectile)
		{
			return new Projectile(projectile);
		}
	}
}