using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Service.Mobs
{
    class Spider : Mob
    {
        const float SPEED = 0.7f;
        //Как сказали, так и сделал :)
        const int RADIUS_MIN = 15;
        const int RADIUS_MAX = 20;

        public Spider() 
        {
            Random rand = new Random();
            Radius = rand.Next(RADIUS_MIN, RADIUS_MAX);
            Speed = SPEED; 
        }
    }
}
