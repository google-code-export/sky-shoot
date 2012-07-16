using System;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon
{
	public class Shotgun : AWeapon
	{
		private readonly Random _rand;

		public Shotgun(Guid id, AGameObject owner = null) : base(id, owner)
		{
            _rand = new Random();
            WeaponType = WeaponType.Shotgun;
            ReloadSpeed = SkyShoot.Contracts.Constants.SHOTGUN_ATTACK_RATE;
		}

		public override AGameObject[] CreateBullets(Vector2 direction)
		{
			var bullets = new ShotgunBullet[8];

			for (int i = 0; i < 8; i++)
			{
				bullets[i] = new ShotgunBullet(
                    Owner,
                    Guid.NewGuid(),
                    Vector2.Transform(direction, Matrix.CreateRotationZ((float) (-Math.PI/6f + _rand.NextDouble()*Math.PI/3f))));
			}

			return bullets;
		}
	}
}