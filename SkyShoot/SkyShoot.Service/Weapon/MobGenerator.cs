using System;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Weapon;
using SkyShoot.ServProgram.Mobs;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon
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
			var mobs = new AGameObject[] { new ChildrenMob() };
			foreach (var mob in mobs)
			{
				mob.Coordinates = new Vector2(Owner.Coordinates.X + Owner.RunVector.X*(Owner.Radius + mob.Radius)*1.5f,
				                              Owner.Coordinates.Y + Owner.RunVector.Y*(Owner.Radius + mob.Radius)*1.5f);
				mob.TeamIdentity = Owner.TeamIdentity;
			}
			return mobs;
		}
	}
}
