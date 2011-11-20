using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Service.Mobs
{
    class Spider : Mob
    {
        public Spider() 
        {
            Radius = 100; // change to real value
            Speed = 100; // change to real value
        }
    }
}
