using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

namespace SkyShoot.Game.Client.Players
{
    class Mob : AMob
    {
        public Mob(Vector2 coordinates, Guid id, Texture2D texture)
            : base(coordinates, id, texture)
        {
            IsPlayer = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Coordinates, Color.White);
        }

    }
}
