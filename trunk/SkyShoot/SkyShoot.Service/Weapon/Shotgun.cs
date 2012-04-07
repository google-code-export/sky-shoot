using System;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;
using SkyShoot.Contracts.Weapon;

namespace SkyShoot.Service.Weapon
{
	public class Shotgun : AWeapon
	{
		private Random _rand;

		private void Init()
		{
			_rand = new Random();
			WheaponType = AWeaponType.Shotgun;
			ReloadSpeed = SkyShoot.Contracts.Constants.SHOTGUN_ATTACK_RATE;
		}

		public Shotgun(Guid id) : base(id)
		{
			Init();
		}

		public Shotgun(Guid id, AGameObject owner) : base(id, owner)
		{
			Init();
		}

		public override AGameObject[] CreateBullets(AGameObject owner, Vector2 direction)
		{
			var bullets = new ShotgunBullet[8];

			for (int i = 0; i < 8; i++)
			{
				bullets[i] = new ShotgunBullet(owner, Guid.NewGuid(),
											   Vector2.Transform(direction,
																 Matrix.CreateRotationZ(
																 	(float) (-Math.PI/6f + _rand.NextDouble()*Math.PI/3f))));
			}

			return bullets;
		}
	}
}