using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

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

        public abstract void Draw(SpriteBatch spriteBatch);

        public virtual void Update(GameTime gameTime)
        {
            int milliseconds = gameTime.ElapsedGameTime.Milliseconds;
            Coordinates += RunVector * Speed * milliseconds;
        }

    }

}
