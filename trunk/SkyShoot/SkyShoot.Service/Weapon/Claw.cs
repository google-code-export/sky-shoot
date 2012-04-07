using System;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Contracts;

namespace SkyShoot.Service.Weapon
{
	//todo //!! 2 delete
	class Claw:AWeapon
	{
		public Claw(Guid id) : base(id) { Owner = null; }

		public Claw(Guid id, AGameObject owner)
			: base(id) 
		{
			if (owner == null) throw new ArgumentNullException("owner");
			Owner = owner;
			ReloadSpeed = Constants.CLAW_ATTACK_RATE;
		}

		public override AGameObject[] CreateBullets(AGameObject owner, SkyShoot.XNA.Framework.Vector2 direction)
		{
			throw new NotImplementedException();
		}
	}
}
