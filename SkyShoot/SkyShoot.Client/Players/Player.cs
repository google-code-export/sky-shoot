using System;
using Microsoft.Xna.Framework;
using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Client.Players
{
    class Player:AMob
    {

        public override bool IsPlayer
        {
            get { return true; }
        }

        public Player(Vector2 coordinates, Guid id):base(coordinates, id)
        {

        }

    }
}
