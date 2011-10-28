using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

using SkyShoot.Contracts.Bonuses.Weapon.Projectiles;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Service.Weapon
{
    public class Shotgun : Weapon
    {
        public Shotgun(Guid id) : base(id) { }

        public override AProjectile[] CreateBullets(AMob owner, PointF direction)
        {
            ShotgunBullet[] bullets = new ShotgunBullet[8];

            double directionAngle = Math.Atan(direction.Y/direction.X);
            double currentAngle;
            for (int i = 0; i < 8; i++)
            {
                currentAngle = directionAngle - Math.PI/6 + (new Random()).NextDouble()*Math.PI/3;
                PointF currentDirection = new PointF((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));
                bullets[i] = new ShotgunBullet(owner, new Guid(), currentDirection);
            }

            return bullets;
        }
    }
}