using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

namespace SkyShoot.Game.Client.Players
{
    class Player : AMob
    {

        public Player(Vector2 coordinates, Guid id, Texture2D texture)
            : base(coordinates, id, texture)
        {
            IsPlayer = true;
            //todo temporary
            Speed = (float)0.5;
            Coordinates = coordinates;
            RunVector = Vector2.Zero;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }
}
