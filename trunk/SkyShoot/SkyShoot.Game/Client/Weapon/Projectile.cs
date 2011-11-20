using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Contracts.Weapon.Projectiles;

using SkyShoot.Game.Client.View;

using IDrawable = SkyShoot.Game.Client.View.IDrawable;

namespace SkyShoot.Game.Client.Weapon
{
    public class Projectile : AProjectile, IDrawable
    {
        public Texture2D Texture { get; private set; }

        public Projectile(AProjectile projectile) : 
            base(projectile)
        {
            //todo
            switch (Type)
            {
                case EnumBulletType.Bullet: 
                    break;
                case EnumBulletType.Flame:
                    break;
                case EnumBulletType.Rocket:
                    break;
            }
            //todo color
            Texture = new Texture2D(Textures.GraphicsDevice, 10, 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float rotation = (float)Math.Atan2(Direction.Y, Direction.X) - MathHelper.PiOver2;

            spriteBatch.Draw(Texture,
                Coordinates,
                null,
                Color.Red,
                rotation,
                new Vector2(Texture.Width / 2f, Texture.Height / 2f),
                1,
                SpriteEffects.None,
                0);
        }
    }
}
