using System;

using SkyShoot.Contracts.Bonuses;

using SkyShoot.Game.Client.Bonuses;
using SkyShoot.Game.Client.Players;
using SkyShoot.Game.Client.View;
using SkyShoot.Game.Client.Weapon;

namespace SkyShoot.Game.Client.Game
{
    class GameFactory
    {

        public static Mob CreateClientMob(Contracts.Mobs.AMob mob)
        {
            //todo mob type
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

        public static Projectile CreateClientProjectile(Contracts.Weapon.Projectiles.AProjectile projectile)
        {
            return new Projectile(projectile);
        }

    }
}
