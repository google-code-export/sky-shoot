using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkyShoot.Game.Client.Players
{
    class Mob : AMob
    {
        public Mob(Vector2 coordinates, Guid id)
            : base(coordinates, id)
        {
            IsPlayer = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

    }
}
