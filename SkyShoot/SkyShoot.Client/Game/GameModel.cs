using System;

using System.Collections.Concurrent;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Client.Players;
using SkyShoot.Client.Weapon;

using SkyShoot.Contracts.Session;

namespace SkyShoot.Client.Game
{
    public class GameModel
    {
        public GameLevel GameLevel { get; private set; }

        public IDictionary<Guid, AMob> Mobs { get; private set;  }

        public IDictionary<Guid, AProjectile> Projectiles { get; private set; }

        public GameModel(TileSet usedTileSet)
        {
            //UsedTileSet = usedTileSet;
            //todo create game level based from tileSet;
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

        public void Draw(SpriteBatch spriteBatch)
        {
            //draw background
            GameLevel.Draw(spriteBatch);

            foreach (var aMob in Mobs)
            {
                aMob.Value.Draw(spriteBatch);
            }

            foreach (var aProjectile in Projectiles)
            {
                aProjectile.Value.Draw(spriteBatch);
            }

        }

    }

}
