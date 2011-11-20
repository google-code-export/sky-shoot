using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Mobs;
using Microsoft.Xna.Framework;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Weapon.Projectiles;
using System.Timers;

namespace SkyShoot.Service.Session
{
	
    public class GameSession
    {
		 //Хотя на сервере ни о каких кадрах речи не идет, всё же думаю, 
        //что это имя очень точно отражает суть константы.
        const int FPS = 1000/60;

		public List<MainSkyShootService> players{get; set;}
        public List<Mob> mobs{get; set;}
        public List<AProjectile> projectiles { get; set; }

        public GameDescription LocalGameDescription { get; private set; }

        public bool IsStarted {get; set; }

        private Timer _gameTimer;
        private GameLevel _gameLevel;
        private SpiderFactory _spiderFactory;
        private long _timerCounter;
        private long _intervalToSpawn = 0;

        public GameSession(TileSet tileSet, int maxPlayersAllowed, GameMode gameType, int gameID, TileSet usedTileSet)
        {
            IsStarted = false;
			_gameLevel = new GameLevel(tileSet);

            players = new List<MainSkyShootService>();

            var playerNames = new List<string>();

			mobs = new List<Mob>();
			players = new List<MainSkyShootService>();
			projectiles = new List<AProjectile>();

            LocalGameDescription = new GameDescription(playerNames, maxPlayersAllowed, gameType, gameID, usedTileSet);
            _spiderFactory= new SpiderFactory(_gameLevel);
        }

        public event SomebodyMovesHandler SomebodyMoves; 
        public event SomebodyShootsHandler SomebodyShoots;
        public event StartGameHandler StartGame;
        public event SomebodyDiesHandler SomebodyDies;
		public event SomebodyHitHandler SomebodyHit;
        public event SomebodySpawnsHandler SomebodySpawns;

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

        public void SomebodySpawned(AMob sender) 
        {
            if (SomebodySpawns != null) 
            {
                SomebodySpawns(sender);
            }
        }

        public void MobDead(Mob mob)
        {
            SomebodyDied(mob);
            mob.MeMoved -= SomebodyMoved;
            mobs.Remove(mob);
        }

		public void SomebodyHitted(AMob mob, AProjectile projectile)
		{
			SomebodyHit(mob, projectile);
		}

		public void PlayerLeave(MainSkyShootService player)
		{
			this.SomebodyHit -= player.Hit;
            player.GameOver();
            SomebodyDied(player);
            player.MeMoved -= SomebodyMoved;
            player.MeShot -= SomebodyShot;
            player.MobSpawned -= SomebodySpawned;
            player.MobDied -= SomebodyDied;
			players.Remove(player);
		}

        public void SpawnMob()
        {
            if (_intervalToSpawn == 0)
            {
                _intervalToSpawn = (long) Math.Exp(4.8 - _timerCounter/40000)+3;
                
                var mob = _spiderFactory.CreateMob();
                mobs.Add(mob);
                SomebodySpawned(mob);
                mob.MeMoved += new SomebodyMovesHandler(SomebodyMoved);
            }
            else
            {
                _intervalToSpawn--;
            }
        }

        // здесь будут производится обработка всех действий
        private void update() 
        {
            SpawnMob();

            foreach(Mob mob in mobs)
            {
                mob.Think(_timerCounter, players);
                mob.Coordinates = ComputeMovement(mob);
            }

            foreach (MainSkyShootService player in players)
            {
                player.Coordinates = ComputeMovement(player);

                //Проверка на касание игрока и моба
                hitTestTouch(player);
            }

            foreach (AProjectile projectile in projectiles)
            {
                var newCord = projectile.Coordinates + projectile.Direction * projectile.Speed;
                
                //Проверка на касание пули и моба
                var hitedMob = hitTestProjectile(projectile, newCord);
                if (hitedMob == null)
                {
                    projectile.Coordinates = newCord;
                    projectile.LifeTime--;
                }
                else
                {
                    hitedMob.DemageTaken(projectile);
                    
                    if (hitedMob.HealthAmount <= 0)
                    {
						MobDead(hitedMob);
                    }
					projectile.LifeTime = 0;
                }
                
            }
			projectiles.RemoveAll(x => (x.LifeTime <= 0));

			if (_timerCounter % 60 == 0)
			{
				var allObjects = new List<AMob>(mobs);
				allObjects.AddRange(players);
				foreach (MainSkyShootService player in players)
				{
					player.SynchroFrame(allObjects.ToArray());
				}
			}
			_timerCounter++;

        }

        private Mob hitTestProjectile(AProjectile projectile, Vector2 newCord)
        {
            var prX = newCord.X - projectile.Coordinates.X;
            var prY = newCord.Y - projectile.Coordinates.Y;

            Mob hitedTarget = null;
            var minDist = double.MaxValue;
         
            foreach(Mob mob in mobs)
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

        private void hitTestTouch(MainSkyShootService player)
        {
            foreach (Mob mob in mobs)
            {
                if ((mob.Coordinates - player.Coordinates).Length() <= mob.Radius + player.Radius)
                {
                    player.HealthAmount -= mob.Damage;
                    if (player.HealthAmount <= 0)
                    {
                        PlayerLeave(player);
                    }
                }
            }
        } 

        public void Start()
        {
			foreach (MainSkyShootService player in players)
			{
				this.SomebodyMoves += new SomebodyMovesHandler(player.MobMoved);
				player.MeMoved += new SomebodyMovesHandler(SomebodyMoved);

				this.SomebodyShoots += new SomebodyShootsHandler(player.MobShot);
				player.MeShot += new ClientShootsHandler(SomebodyShot);

                this.SomebodySpawns += new SomebodySpawnsHandler(player.SpawnMob);

                this.SomebodyDies += new SomebodyDiesHandler(player.MobDead);
                
				this.SomebodyHit += player.Hit;

				player.Coordinates = new Vector2(0,0);
				player.Speed = 100;
				player.Weapon = new Weapon.Pistol(new Guid());
				player.RunVector = new Vector2(0, 0);
			}

			StartGame(players.ToArray(), _gameLevel);
        }

		public bool AddPlayer(MainSkyShootService player)
		{
			if (players.Count >= LocalGameDescription.MaximumPlayersAllowed)
				return false;

			if (IsStarted) return false;

			players.Add(player);
			LocalGameDescription.Players.Add(player.Name);
			StartGame += new StartGameHandler(player.GameStart);
			if (players.Count >= LocalGameDescription.MaximumPlayersAllowed)
			{
				
				_gameTimer = new Timer(FPS);
				_gameTimer.AutoReset = true;
				_gameTimer.Elapsed += new ElapsedEventHandler(TimerElapsedListener);
				_gameTimer.Start();
				_timerCounter = 0;
			}
			return true;
		}

        private void TimerElapsedListener(object sender,EventArgs e)
        {
			if (!IsStarted) Start(); 
			IsStarted = true;
			update();
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