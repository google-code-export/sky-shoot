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

        public IDictionary<Guid, Mob> Mobs { get; private set;  }

        public IDictionary<Guid, Projectile> Projectiles { get; private set; }

        public Camera2D Camera2D { get; private set; }

        public GameModel(GameLevel gameLevel)
        {
            GameLevel = gameLevel;

            Camera2D = new Camera2D(GameLevel.Width, GameLevel.Height);

            Mobs = new ConcurrentDictionary<Guid, Mob>();
            Projectiles = new ConcurrentDictionary<Guid, Projectile>();
        }

        public void AddMob(Mob mob)
        {
            Mobs.Add(mob.Id, mob);
        }

        public void AddProjectile(Projectile projectile)
        {
            Projectiles.Add(projectile.Id, projectile);
        }

        public Mob GetMob(Guid id)
        {
            return Mobs[id];
        }

        public Projectile GetProjectile(Guid id)
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
            // update mobs
            foreach (var aMob in Mobs)
            {
                aMob.Value.Update(gameTime);
            }

            // update projectiles
            foreach (var projectile in Projectiles)
            {
                if(projectile.Value.IsActive)
                    projectile.Value.Update(gameTime);
                else
                    RemoveProjectile(projectile.Value.Id);
            }
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
