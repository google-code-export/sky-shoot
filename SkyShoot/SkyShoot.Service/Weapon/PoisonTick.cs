using System;
using SkyShoot.Contracts;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon
{
	public class PoisonTick : AWeapon
	{
		public PoisonTick(Guid id, AGameObject owner = null)
			: base(id, owner)
		{
			WeaponType = WeaponType.PoisonTick;
			ReloadSpeed = Constants.POISONTICK_ATTACK_RATE;
		}

		public override AGameObject[] CreateBullets(AGameObject owner, Vector2 direction)
		{
			var bullets = new[] { new PistolBullet(owner, Guid.NewGuid(), direction) };
			return bullets;
		}
	}
}