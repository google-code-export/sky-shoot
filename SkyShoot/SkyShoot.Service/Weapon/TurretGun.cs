using System;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon
{
	public class TurretGun : AWeapon
	{
		private readonly Vector2 _parentTurretCoordinates;

		public TurretGun(Guid id, Vector2 parentTurretCoordinates, AGameObject owner = null)
			: base(id, owner)
		{
			WeaponType = WeaponType.TurretGun;
			ReloadSpeed = Constants.TURRET_GUN_ATTACK_RATE;
			this._parentTurretCoordinates = parentTurretCoordinates;
		}

		public override AGameObject[] CreateBullets(Vector2 direction)
		{
			var bullets = new AGameObject[] { new TurretGunBullet(Owner, Guid.NewGuid(), direction, _parentTurretCoordinates) };
			return bullets;
		}
	}
}
