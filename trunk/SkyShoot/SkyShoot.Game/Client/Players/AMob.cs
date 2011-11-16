using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using SkyShoot.Game.Client.Game;
using IDrawable = SkyShoot.Game.Client.View.IDrawable;

namespace SkyShoot.Game.Client.Players
{
    public abstract class AMob : Contracts.Mobs.AMob, IDrawable
    {
        public Texture2D Texture { get; set; }

        protected AMob(Vector2 coordinates, Guid id, Texture2D texture) : base(coordinates, id)
        {
            Texture = texture;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            float rotation = (float)Math.Atan2(ShootVector.Y, ShootVector.X) - MathHelper.PiOver2;

            spriteBatch.Draw(Texture,
                Coordinates,
                null,
                Color.White,
                rotation,
                new Vector2(Texture.Width / 2f, Texture.Height / 2f),
                1,
                SpriteEffects.None,
                0);
        } 

        public virtual void Update(GameTime gameTime)
        {
            int milliseconds = gameTime.ElapsedGameTime.Milliseconds;
            Coordinates += RunVector * Speed * milliseconds;

            Coordinates.X = MathHelper.Clamp(Coordinates.X, 0, GameLevel.Width);
            Coordinates.Y = MathHelper.Clamp(Coordinates.Y, 0, GameLevel.Height);
        }

    }

}
