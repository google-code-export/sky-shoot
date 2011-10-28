using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Service.Weapon.Bullets
{
    public class ShotgunBullet : ABullet
    {
        public ShotgunBullet(AMob owner, Guid id, PointF direction)
            : base(owner, id, direction, 10, 2, EnumBulletType.Bullet) { }
    }
}