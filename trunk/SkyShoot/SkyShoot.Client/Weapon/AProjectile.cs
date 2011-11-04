using System;
using Microsoft.Xna.Framework.Graphics;
using SkyShoot.Client.Players;
using SkyShoot.Client.View;

namespace SkyShoot.Client.Weapon
{
    public abstract class AProjectile : Contracts.Weapon.Projectiles.AProjectile, IDrawable
    {

        protected AProjectile(AMob owner, Guid id) : base(owner, id)
        {
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
