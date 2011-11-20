using System;
using System.Threading;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;

using SkyShoot.Contracts.Weapon.Projectiles;
using Timer = System.Timers.Timer;

namespace SkyShoot.Service.Session
{
	
    public class GameSession
    {
		 //Хотя на сервере ни о каких кадрах речи не идет, всё же думаю, 
        //что это имя очень точно отражает суть константы.
        const int FPS = 1000/60;

		public List<MainSkyShootService> Players {get; set;}
        public List<Mob> Mobs {get; set;}
        public List<AProjectile> Projectiles { get; set; }

        public GameDescription LocalGameDescription { get; private set; }

        public bool IsStarted {get; set; }

        private Timer _gameTimer;
        private readonly GameLevel _gameLevel;
        private readonly SpiderFactory _spiderFactory;
        private long _timerCounter;
        private long _intervalToSpawn;

		public GameSession(TileSet tileSet, int maxPlayersAllowed, GameMode gameType, int gameId)
        {
		    _intervalToSpawn = 0;
		    IsStarted = false;
			_gameLevel = new GameLevel(tileSet);

            Players = new List<MainSkyShootService>();
            Mobs = new List<Mob>();
            Projectiles = new List<AProjectile>();

            var playerNames = new List<string>();

			LocalGameDescription = new GameDescription(playerNames, maxPlayersAllowed, gameType, gameId);
            _spiderFactory= new SpiderFactory(_gameLevel);
        }

        public event SomebodyMovesHandler SomebodyMoves; 
        public event SomebodyShootsHandler SomebodyShoots;
        public event StartGameHandler StartGame;
        public event SomebodyDiesHandler SomebodyDies;
		public event SomebodyHitHandler SomebodyHit;

        private void SomebodyMoved(AMob sender, Vector2 direction)
        {
            sender.RunVector = direction;
            if (SomebodyMoves != null)
            {
                SomebodyMoves(sender, direction);
            }
        }

        private void SomebodyShot(AMob sender, Vector2 direction)
        {
            sender.ShootVector = direction;
            if (SomebodyShoots != null)
            {
                if ((sender as MainSkyShootService).Weapon != null)
                    SomebodyShoots(sender, 
                        (sender as MainSkyShootService).Weapon.CreateBullets(sender, direction));
            }
        }

        public void SomebodyDied(AMob sender)
        {
            if (SomebodyDies != null)
            {
                SomebodyDies(sender);
            }
        }

        public void MobDead(Mob mob)
        {
            SomebodyDied(mob);
            mob.MeMoved -= SomebodyMoved;
            Mobs.Remove(mob);
        }

        public void PlayerDead(MainSkyShootService player)
        {
            player.GameOver();
            SomebodyDied(player);
			PlayerLeave(player);
        }

		public void SomebodyHitted(AMob mob, AProjectile projectile)
		{
			SomebodyHit(mob, projectile);
		}

		public void PlayerLeave(MainSkyShootService player)
		{
			SomebodyHit -= player.Hit;
			//здесь вся отписка от событий для player

			Players.Remove(player);
		}

        public void SpawnMob()
        {
            if (_intervalToSpawn == 0)
            {
                _intervalToSpawn = (long) Math.Exp(4.8 - _timerCounter/40000)+3;
                
                var mob = _spiderFactory.CreateMob();
                Mobs.Add(mob);
                mob.MeMoved += SomebodyMoved;
            }
            else
            {
                _intervalToSpawn--;
            }
        }

        // здесь будут производится обработка всех действий
        private void Update() 
        {
            SpawnMob();

            foreach(Mob mob in Mobs)
            {
                mob.Think(_timerCounter, Players);
                mob.Coordinates = ComputeMovement(mob);
            }

            foreach (MainSkyShootService player in Players)
            {
                player.Coordinates = ComputeMovement(player);

                //Проверка на касание игрока и моба
                HitTestTouch(player);
            }

            foreach (AProjectile projectile in Projectiles)
            {
                var newCord = projectile.Coordinates + projectile.Direction * projectile.Speed;
                
                //Проверка на касание пули и моба
                var hitedMob = HitTestProjectile(projectile, newCord);
                if (hitedMob == null)
                {
                    projectile.Coordinates = newCord;
                    projectile.Timer--;
                }
                else
                {
                    hitedMob.DemageTaken(projectile);
                    
                    if (hitedMob.HealthAmount <= 0)
                    {
						MobDead(hitedMob);
                    }
					projectile.Timer = 0;
                }
                
            }
			Projectiles.RemoveAll(x => (x.Timer <= 0));
			
			if (_timerCounter % 60 == 0 )
				foreach (MainSkyShootService player in Players)
				{
					player.SynchroFrame(Mobs.ToArray());
				}
			_timerCounter++;

        }

        private Mob HitTestProjectile(AProjectile projectile, Vector2 newCord)
        {
            var prX = newCord.X - projectile.Coordinates.X;
            var prY = newCord.Y - projectile.Coordinates.Y;

            Mob hitedTarget = null;
            var minDist = double.MaxValue;
         
            foreach(Mob mob in Mobs)
            {
                var mX = mob.Coordinates.X - projectile.Coordinates.X;
                var mY = mob.Coordinates.Y - projectile.Coordinates.Y;
                var mR = mob.Radius;
                var mDist = Math.Sqrt(mX * mX + mY * mY);

                if (mDist <= mR && mDist < minDist)
                {
                    hitedTarget = mob;
                    minDist = mDist;
                    continue;
                }

                if (prX == 0 && prY == 0)
                {
                    continue;
                }

                var h = (prX * mY - prY * mX) / Math.Sqrt(prX * prX * + prY * prY);

                //@TODO Проверка углов. Над ней еще надо будет подумать.
                var cos1 = mX * prX + mY * prY;
                var cos2 = -1 * (prX * (mX - prX) + prY * (mY - prY));

                if (h <= mR && Math.Sign(cos1) == Math.Sign(cos2) && mDist < minDist)
                {
                    hitedTarget = mob;
                    minDist = mDist;
                }

            }

            return hitedTarget;
        }

        private void HitTestTouch(MainSkyShootService player)
        {
            foreach (Mob mob in Mobs)
            {
                if ((mob.Coordinates - player.Coordinates).Length() <= mob.Radius + player.Radius)
                {
                    player.HealthAmount -= mob.Damage;
                    if (player.HealthAmount <= 0)
                    {
                        PlayerDead(player);
                    }
                }
            }
        }

        public bool Start()
        {
			if (IsStarted) return false;
            IsStarted = true;

			foreach (MainSkyShootService player in Players)
			{
				SomebodyMoves += player.MobMoved;
				player.MeMoved += SomebodyMoved;

				SomebodyShoots += player.MobShot;
				player.MeShot += SomebodyShot;

				SomebodyHit += player.Hit;

				player.Coordinates = new Vector2(0,0);
				player.Speed = 100;
				player.Weapon = new Weapon.Pistol(new Guid());
				player.RunVector = new Vector2(0, 0);
			}

            _gameTimer = new Timer(FPS);
            _gameTimer.AutoReset = true;
            _gameTimer.Elapsed += TimerElapsedListener;
			_gameTimer.Start();
            _timerCounter = 0;

            return true;
        }

		public bool AddPlayer(MainSkyShootService player)
		{
			if (Players.Count >= LocalGameDescription.MaximumPlayersAllowed)
				return false;

			Players.Add(player);
			LocalGameDescription.Players.Add(player.Name);
			StartGame += player.GameStart;
            //todo temporary solution
			if (Players.Count >= LocalGameDescription.MaximumPlayersAllowed)
			{
                Thread newThread = new Thread(Send);
                newThread.Start();
			}
			return true;
		}

        private void Send()
        {                
            //todo Players + Mobs;
            StartGame(Players.ToArray(), _gameLevel);
            Start();            
        }

        private void TimerElapsedListener(object sender,EventArgs e)
        {
			Update();
        }

        private Vector2 ComputeMovement(AMob mob)
		{
			var realHeight=_gameLevel.levelHeight-mob.Radius;
			var realWidth=_gameLevel.levelWidth-mob.Radius;
			var newCoord = new Vector2(mob.Coordinates.X + mob.Speed * FPS * mob.RunVector.X, mob.Coordinates.Y + mob.Speed * FPS * mob.RunVector.Y);
			if(Math.Abs(mob.Coordinates.X) <= realWidth)
				newCoord.X=Math.Min(Math.Abs(newCoord.X), realWidth) * Math.Sign(newCoord.X);
			else
				newCoord.X=Math.Min(Math.Abs(newCoord.X), realWidth + _gameLevel.LEVELBORDER) * Math.Sign(newCoord.X);
				
			if (Math.Abs(mob.Coordinates.Y) <= realHeight)
				newCoord.Y = Math.Min(Math.Abs(newCoord.Y), realHeight) * Math.Sign(newCoord.Y);
			else
				newCoord.Y = Math.Min(Math.Abs(newCoord.Y), realHeight + _gameLevel.LEVELBORDER) * Math.Sign(newCoord.Y);

			return newCoord;
		}
    }
}