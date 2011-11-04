using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Client.View;

namespace SkyShoot.Client.Players
{
    class Player : AMob
    {

        //public bool IsPlayer
        //{
        //    set;
        //    get { return true; }
        //}

        public Player(Vector2 coordinates, Guid id)
            : base(coordinates, id)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

    }
}
