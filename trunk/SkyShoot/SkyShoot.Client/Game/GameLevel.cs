using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Weapon.Projectiles;

namespace SkyShoot.Client.Game
{
    class GameLevel
    {

        public TileSet UsedTileSet { get; private set; }

        public IDictionary<Guid, AMob> Mobs { get; private set;  }

        public IDictionary<Guid, AProjectile> Projectiles { get; private set; }

        public GameLevel(TileSet usedTileSet)
        {
            UsedTileSet = usedTileSet;
            Mobs = new ConcurrentDictionary<Guid, AMob>();
            Projectiles = new ConcurrentDictionary<Guid, AProjectile>();
        }

        public void AddMob(AMob mob)
        {
            Mobs.Add(mob.Id, mob);
        }

        public void AddProjectile(AProjectile projectile)
        {
            Projectiles.Add(projectile.Id, projectile);
        }

        public AMob GetMob(Guid id)
        {
            return Mobs[id];
        }

        public AProjectile GetProjectile(Guid id)
        {
            return Projectiles[id];
        }

        public void RemoveMob(Guid id)
        {
            Mobs.Remove(id);
        }

        public void RemoveProjectile(Guid id)
        {
            Projectiles.Remove(id);
        }

    }
}
