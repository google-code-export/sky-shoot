using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Weapon.Projectiles;
using Microsoft.Xna.Framework;

namespace SkyShoot.Contracts.Service
{
    public interface ISkyShootCallback
    {
        void GameStart(AMob[] mobs, GameLevel arena);

        void Shoot(AProjectile projectile); // лучше передавать массив AProjectile[] projectiles

        void SpawnMob(AMob mob);

        void Hit(AMob mob, AProjectile projectile);

        void MobMoved(AMob mob, Vector2 direction);

        void MobDead(AMob mob);

        void BonusDropped(AObtainableDamageModifier bonus);

        void BonusExpired(AObtainableDamageModifier bonus);

        void BonusDisappeared(AObtainableDamageModifier bonus);

        void GameOver();

        void PlayerLeft(AMob mob);

        void SynchroFrame(AMob[] mobs);
    }
}
