using System;
using Microsoft.Xna.Framework.Graphics;
using SkyShoot.Game.Client.Players;
using SkyShoot.Game.Client.View;

namespace SkyShoot.Game.Client.Weapon
{
    public abstract class AProjectile : Contracts.Weapon.Projectiles.AProjectile, IDrawable
    {

        protected AProjectile(AMob owner, Guid id) : base(owner, id)
        {
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
