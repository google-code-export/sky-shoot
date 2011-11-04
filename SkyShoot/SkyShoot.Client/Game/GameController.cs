using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon.Projectiles;

namespace SkyShoot.Client.Game
{
    class GameController:Contracts.Service.ISkyShootCallback
    {
        private GameView _gameView;
        private GameModel _arena;

        private GameController()
        {
            _gameView = new GameView();
        }

        public void GameStart(AMob[] mobs, Contracts.Session.GameLevel arena)
        {
            _arena = GameFactory.CreateClientGameLevel(arena);
            foreach (AMob mob in mobs)
            {
                var clientMob = GameFactory.CreateClientMob(mob);
                _arena.AddMob(clientMob);
            }
        }

        public void Shoot(AProjectile projectile)
        {
            _arena.AddProjectile(GameFactory.CreateClientProjectile(projectile));
        }

        public void SpawnMob(AMob mob)
        {
            throw new System.NotImplementedException();
        }

        public void Hit(AMob mob, AProjectile projectile)
        {
            //todo
            _arena.GetMob(mob.Id).HealthAmount -= 10;
        }

        public void MobDead(AMob mob)
        {
            _arena.RemoveMob(mob.Id);
        }

        public void BonusDropped(AObtainableDamageModifier bonus)
        {
            throw new System.NotImplementedException();
        }

        public void BonusExpired(AObtainableDamageModifier bonus)
        {
            throw new System.NotImplementedException();
        }

        public void BonusDisappeared(AObtainableDamageModifier bonus)
        {
            throw new System.NotImplementedException();
        }

        public void GameOver()
        {
            throw new System.NotImplementedException();
        }

        public void PlayerLeft(AMob mob)
        {
            throw new System.NotImplementedException();
        }

        public void SynchroFrame(AMob[] mob)
        {
            throw new System.NotImplementedException();
        }
    }
}
