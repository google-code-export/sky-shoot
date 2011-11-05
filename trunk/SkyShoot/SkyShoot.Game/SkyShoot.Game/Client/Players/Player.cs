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
            Coordinates = new Vector2(100, 100);
            RunVector = Vector2.Zero;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Coordinates, Color.White);
        }

    }
}
