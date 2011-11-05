using System;

using SkyShoot.Client.Bonuses;
using SkyShoot.Client.Players;
using SkyShoot.Client.Weapon;

using SkyShoot.Contracts.Bonuses;

namespace SkyShoot.Client.Game
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
