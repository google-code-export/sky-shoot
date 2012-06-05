using System;
using SkyShoot.Contracts;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon
{
	public class SpiderWeapon : AWeapon
	{
		public SpiderWeapon(Guid newGuid) : base(newGuid)
		{
			WeaponType = WeaponType.SpiderPistol;
			ReloadSpeed = Constants.PISTOL_ATTACK_RATE;
		}

		public override AGameObject[] CreateBullets(AGameObject owner, Vector2 direction)
		{
			var bullets = new[] { new SpiderBullet(owner, Guid.NewGuid(), direction) };
			return bullets;
		}
	}
}