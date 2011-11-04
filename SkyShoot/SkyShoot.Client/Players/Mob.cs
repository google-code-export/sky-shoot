using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyShoot.Client.View;

namespace SkyShoot.Client.Players
{
    class Mob : AMob
    {
        public override bool IsPlayer
        {
            get { return false; }
        }

        public Mob(Vector2 coordinates, Guid id)
            : base(coordinates, id)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

    }
}
