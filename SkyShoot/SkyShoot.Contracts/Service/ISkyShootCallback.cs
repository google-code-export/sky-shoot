using System.ServiceModel;

using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Bonuses;

using SkyShoot.Contracts.Weapon.Projectiles;

using Microsoft.Xna.Framework;

namespace SkyShoot.Contracts.Service
{
    public interface ISkyShootCallback
    {
        [OperationContract(IsOneWay = true)]
        void GameStart(AMob[] mobs, GameLevel arena);

        [OperationContract(IsOneWay = true)]
        void MobShot(AMob mob, AProjectile[] projectiles); // лучше передавать массив AProjectile[] projectiles

        [OperationContract(IsOneWay = true)]
        void SpawnMob(AMob mob);

        [OperationContract(IsOneWay = true)]
        void Hit(AMob mob, AProjectile projectile);

        [OperationContract(IsOneWay = true)]
        void MobMoved(AMob mob, Vector2 direction);

        [OperationContract(IsOneWay = true)]
        void MobDead(AMob mob);

        [OperationContract(IsOneWay = true)]
        void BonusDropped(AObtainableDamageModifier bonus);

        [OperationContract(IsOneWay = true)]
        void BonusExpired(AObtainableDamageModifier bonus);

        [OperationContract(IsOneWay = true)]
        void BonusDisappeared(AObtainableDamageModifier bonus);

        [OperationContract(IsOneWay = true)]
        void GameOver();

        [OperationContract(IsOneWay = true)]
        void PlayerLeft(AMob mob);

        [OperationContract(IsOneWay = true)]
        void SynchroFrame(AMob[] mobs);

    }
}
