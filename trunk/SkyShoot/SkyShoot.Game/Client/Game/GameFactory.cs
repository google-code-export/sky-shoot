using System;

using SkyShoot.Contracts.Bonuses;
using SkyShoot.Game.Client.GameObjects;
using SkyShoot.Game.Client.View;

namespace SkyShoot.Game.Client.Game
{
	internal class GameFactory
	{
		public static Mob CreateClientMob(Contracts.Mobs.AGameObject mob)
		{
			// todo mob type
			return new Mob(mob, mob.IsPlayer ? Textures.PlayerAnimation : Textures.SpiderAnimation);
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
