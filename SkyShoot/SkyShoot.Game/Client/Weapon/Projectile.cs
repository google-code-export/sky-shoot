using System;

using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Game.Client.View;
using SkyShoot.Game.Client.Players;

namespace SkyShoot.Game.Client.Weapon
{
    public class Projectile : Contracts.Weapon.Projectiles.AProjectile, IDrawable
    {
        public Projectile(AMob owner, Guid id) : base(owner, id)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
