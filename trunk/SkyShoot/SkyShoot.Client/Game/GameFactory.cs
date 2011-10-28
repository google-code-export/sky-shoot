using System;

using SkyShoot.Client.Game;
using SkyShoot.Client.Players;

using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon.Projectiles;

namespace SkyShoot.Client.game
{
    class GameFactory
    {

        public static AMob CreateClientMob(AMob mob)
        {
            if (mob.IsPlayer)
                return new Player(mob.Coordinates, mob.Id);
            return null;
        }

        public static GameLevel CreateClientGameLevel(Contracts.Session.GameLevel gameLevel)
        {
            return new GameLevel(gameLevel.UsedTileSet);
        }
 
        public static AObtainableDamageModifier CreateClientBonus(AObtainableDamageModifier bonus)
        {
            throw new NotImplementedException();
        }

        public static AProjectile CreateClientProjectile(AProjectile projectile)
        {
            throw new NotImplementedException();
        }



    }
}
