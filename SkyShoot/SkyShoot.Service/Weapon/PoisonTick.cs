using System;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Service;
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

		public override AGameObject[] CreateBullets(Vector2 direction)
		{
			var bullets = new AGameObject[] { new PistolBullet(Owner, Guid.NewGuid(), direction) };
			return bullets;
		}
	}
}