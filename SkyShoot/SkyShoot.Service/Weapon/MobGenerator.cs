using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.Contracts;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Weapon;
using SkyShoot.ServProgram.Mobs;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.XNA.Framework;

namespace SkyShoot.ServProgram.Weapon
{
	class MobGenerator : AWeapon
	{
		public MobGenerator(Guid id, AGameObject owner = null)
			: base(id, owner)
		{
			WeaponType = WeaponType.MobGenerator;
			ReloadSpeed = Constants.MOB_GENERATOR_ATTACK_RATE;
		}

		public override AGameObject[] CreateBullets(Vector2 direction)
		{
			var mobs = new[] { new ChildrenMob() };
			foreach(var mob in mobs)
				mob.Coordinates = new Vector2(Owner.Coordinates.X + mob.RunVector.X * mob.Radius, Owner.Coordinates.Y + mob.RunVector.Y * mob.Radius);
			return mobs;
		}
	}
}
