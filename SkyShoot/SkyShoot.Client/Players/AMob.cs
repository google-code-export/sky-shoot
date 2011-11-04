using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyShoot.Client.View;

namespace SkyShoot.Client.Players
{
    public abstract class AMob : Contracts.Mobs.AMob, IDrawable
    {
        public abstract void Draw(SpriteBatch spriteBatch);

        protected AMob(Vector2 coordinates, Guid id):base(coordinates, id)
        {

        }

    }

}
