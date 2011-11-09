using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyShoot.Contracts.Mobs
{
    public class Mob : AMob
    {
        public string targetPlayer { get; set; }

        public Mob()
        {
            IsPlayer = false;
        }
    }
}
