using System;

using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Game.Client.View;
using SkyShoot.Game.Client.Players;

namespace SkyShoot.Game.Client.Weapon
{
    public class Projectile : Contracts.Weapon.Projectiles.AProjectile, IDrawable
    {
        private const float SPEED = 10;
        private const float DAMAGE = 2;
        private const int LIFE_TIME = 120;
        private const EnumBulletType TYPE = EnumBulletType.Bullet;

        public Projectile(AMob owner, Guid id, Microsoft.Xna.Framework.Vector2 direction) 
            : base(owner, id, direction, SPEED, DAMAGE, LIFE_TIME, TYPE)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
