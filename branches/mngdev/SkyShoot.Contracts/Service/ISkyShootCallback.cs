using System;
using System.ServiceModel;

using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Bonuses;

using SkyShoot.Contracts.Weapon.Projectiles;

using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.Service
{
	public interface ISkyShootCallback
	{
		[OperationContract(IsOneWay = true)]
		void GameStart(AGameObject[] mobs,GameLevel arena);

		[OperationContract(IsOneWay = true)]
		void MobShot(AGameObject mob, AProjectile[] projectiles);

		[OperationContract(IsOneWay = true)]
		void SpawnMob(AGameObject mob);

		[OperationContract(IsOneWay = true)]
		void Hit(AGameObject mob, AProjectile projectile);

		[OperationContract(IsOneWay = true)]
		void MobMoved(AGameObject mob, Vector2 direction);

		[OperationContract(IsOneWay = true)]
		void MobDead(AGameObject mob);

		[OperationContract(IsOneWay = true)]
		void BonusDropped(AObtainableDamageModifier bonus);

		[OperationContract(IsOneWay = true)]
		void BonusExpired(AObtainableDamageModifier bonus);

		[OperationContract(IsOneWay = true)]
		void BonusDisappeared(AObtainableDamageModifier bonus);

		[OperationContract(IsOneWay = true)]
		void GameOver();

		[OperationContract(IsOneWay = true)]
		void PlayerLeft(AGameObject mob);

		[OperationContract(IsOneWay = true)]
		void SynchroFrame(AGameObject[] mobs);

		[OperationContract(IsOneWay = true)]
		void PlayerListChanged(String[] names);

	}
}
