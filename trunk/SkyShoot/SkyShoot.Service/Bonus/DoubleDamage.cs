using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkyShoot.Service.Bonus
{
    public class DoubleDamage : ABonus
    {
        public DoubleDamage(Guid id, DateTime startTime)
            : base(id, 1, 2, 30000, startTime) { }
    }
}