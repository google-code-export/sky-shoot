using System;

using System.Collections.Concurrent;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Game.Client.Players;
using SkyShoot.Game.Client.View;
using SkyShoot.Game.Client.Weapon;

namespace SkyShoot.Game.Client.Game
{
    public class GameModel
    {
        public GameLevel GameLevel { get; private set; }

        public IDictionary<Guid, AMob> Mobs { get; private set;  }

        public IDictionary<Guid, AProjectile> Projectiles { get; private set; }

        public Camera2D Camera2D { get; private set; }

        public GameModel(GameLevel gameLevel)
        {
            GameLevel = gameLevel;

            Camera2D = new Camera2D(GameLevel.Width, GameLevel.Height);

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

        public void Update(GameTime gameTime)
        {
            //Update Mobs
            foreach (var aMob in Mobs)
            {
                aMob.Value.Update(gameTime);
            }

            //todo update projectiles
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            Vector2 myPosition = GetMob(GameController.MyId).Coordinates;

            Camera2D.Position = myPosition;

            spriteBatch.Begin(SpriteSortMode.Immediate, 
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                Camera2D.GetTransformation(Textures.GraphicsDevice));

            // draw background
            GameLevel.Draw(spriteBatch);

            // draw mobs
            foreach (var aMob in Mobs)
            {
                aMob.Value.Draw(spriteBatch);
            }

            // draw projectiles
            foreach (var aProjectile in Projectiles)
            {
                aProjectile.Value.Draw(spriteBatch);
            }

            spriteBatch.End();

        }

    }

}
