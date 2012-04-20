using System;
using SkyShoot.Contracts;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;
using SkyShoot.Contracts.Weapon;

namespace SkyShoot.Service.Weapon
{
	public class Pistol : AWeapon
	{
		public Pistol(Guid id) : base(id)
		{
			WeaponType = AWeaponType.Pistol;
			ReloadSpeed = Constants.PISTOL_ATTACK_RATE;
		}

		public Pistol(Guid id, AGameObject owner) : base(id, owner)
		{
			WeaponType = AWeaponType.Pistol;
			ReloadSpeed = Constants.PISTOL_ATTACK_RATE;
		}

		public override AGameObject[] CreateBullets(AGameObject owner, Vector2 direction)
		{
			var bullets = new[] { new PistolBullet(owner, Guid.NewGuid(), direction) };
			return bullets;
		}
	}
}