using System;
using SkyShoot.Contracts.Bonuses;
using SkyShoot.Game.Client.Bonuses;
using SkyShoot.Game.Client.Players;
using SkyShoot.Game.Client.Weapon;

namespace SkyShoot.Game.Client.Game
{
    class GameFactory
    {

        public static AMob CreateClientMob(Contracts.Mobs.AMob mob)
        {
            if (mob.IsPlayer)
                return new Player(mob.Coordinates, mob.Id);
            return null;
        }

        public static GameLevel CreateClientGameLevel(Contracts.Session.GameLevel gameLevel)
        {
            return new GameLevel(gameLevel.UsedTileSet);
        }
 
        public static ABonus CreateClientBonus(AObtainableDamageModifier bonus)
        {
            throw new NotImplementedException();
        }

        public static AProjectile CreateClientProjectile(Contracts.Weapon.Projectiles.AProjectile projectile)
        {
            throw new NotImplementedException();
        }

    }
}
