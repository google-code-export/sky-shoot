using System;
using SkyShoot.Contracts;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.XNA.Framework;
using System.Diagnostics;

namespace SkyShoot.Service.Weapon
{
	public class TurretMaker : AWeapon
	{
		public TurretMaker(Guid id, AGameObject owner = null)
			: base(id, owner)
		{
			WeaponType = WeaponType.TurretMaker;
			//TODO: set reload speed
			ReloadSpeed = 1;
		}

		public override AGameObject[] CreateBullets(AGameObject owner, Vector2 direction)
		{
			AWeapon weapon = new Pistol(Guid.NewGuid());
			var turret = new Turret(100, weapon, 10, owner, Vector2.Add(owner.Coordinates, new Vector2(0, 20)));
			Trace.WriteLine(turret.Id.ToString(), "turret id");
			Trace.WriteLine(owner.Id.ToString(), "turret owner id");
			return new AGameObject[] { turret };
		}
	}
}
