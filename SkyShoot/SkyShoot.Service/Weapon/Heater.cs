using System;
using SkyShoot.Contracts;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon
{
	public class Heater : AWeapon
	{
		public Heater(Guid id, AGameObject owner = null) : base(id, owner)
		{
			WeaponType = WeaponType.Heater;
			ReloadSpeed = Constants.HEATER_ATTACK_RATE;
		}

		public override AGameObject[] CreateBullets(Vector2 direction)
		{
			return new AGameObject[] { new HeaterBullet(Owner, Guid.NewGuid(), direction) };
		}
	}
}