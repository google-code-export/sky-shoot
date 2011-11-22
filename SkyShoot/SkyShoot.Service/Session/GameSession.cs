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
using SkyShoot.Contracts;

namespace SkyShoot.Service.Session
{
	
    public class GameSession
    {
		public List<MainSkyShootService> Players{get; set;}

        private List<Mob> _mobs{get; set;}
        private List<AProjectile> _projectiles { get; set; }

        public GameDescription LocalGameDescription { get; private set; }

        public bool IsStarted { get; set; }		
        private GameLevel _gameLevel;
        private SpiderFactory _spiderFactory;
        private long _timerCounter;
        private long _intervalToSpawn = 0;

		private Timer _gameTimer;

        public GameSession(TileSet tileSet, int maxPlayersAllowed, GameMode gameType, int gameID)
        {
            IsStarted = false;
			_gameLevel = new GameLevel(tileSet);

            var playerNames = new List<string>();

			_mobs = new List<Mob>();
			Players = new List<MainSkyShootService>();
			_projectiles = new List<AProjectile>();

            LocalGameDescription = new GameDescription(playerNames, maxPlayersAllowed, gameType, gameID, tileSet);
            _spiderFactory= new SpiderFactory(_gameLevel);
        }

        public event SomebodyMovesHandler SomebodyMoves; 
        public event SomebodyShootsHandler SomebodyShoots;
        public event StartGameHandler StartGame;
        public event SomebodyDiesHandler SomebodyDies;
		public event SomebodyHitHandler SomebodyHit;
        public event SomebodySpawnsHandler SomebodySpawns;
		public event NewPlayerConnectedHandler NewPlayerConnected;
		
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
				{
					var a = (sender as MainSkyShootService).Weapon.CreateBullets(sender, direction);
					SomebodyShoots(sender, a);
					_projectiles.AddRange(a);
				}
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
            _mobs.Remove(mob);
        }

		public void SomebodyHitted(AMob mob, AProjectile projectile)
		{
			if(SomebodyHit != null)	SomebodyHit(mob, projectile);
		}

		public void PlayerLeave(MainSkyShootService player)
		{
			LocalGameDescription.Players.Remove(player.Name);
			this.SomebodyHit -= player.Hit;
			this.SomebodyMoves -= player.MobMoved;
			this.SomebodyShoots -= player.MobShot;
			this.SomebodySpawns -= player.SpawnMob;
			this.SomebodyDies -= player.MobDead;
			this.NewPlayerConnected -= player.NewPlayerConnected;
			this.StartGame -= player.GameStart;
			
            player.GameOver();
            SomebodyDied(player);
            player.MeMoved -= SomebodyMoved;
            player.MeShot -= SomebodyShot;
            //player.MobSpawned -= SomebodySpawned;
            //player.MobDied -= SomebodyDied;
			
			Players.Remove(player);
		}

		public void Stop()
		{
			_gameTimer.Enabled = false;
			_gameTimer.AutoReset = false;
			_gameTimer.Elapsed -= TimerElapsedListener;
			IsStarted = false;
		}

        public void Start()
        {
            

			foreach (MainSkyShootService player in Players)
			{
				this.SomebodyMoves += player.MobMoved;
				player.MeMoved += SomebodyMoved;

				this.SomebodyShoots += player.MobShot;
				player.MeShot += SomebodyShot;

                this.SomebodySpawns += player.SpawnMob;

                this.SomebodyDies += player.MobDead;
                
				this.SomebodyHit += player.Hit;

				player.Coordinates = new Vector2(50,50);
                player.Speed = Constants.PLAYER_SPEED;
                player.Radius = Constants.PLAYER_RADIUS;
				player.Weapon = new Weapon.Pistol(new Guid());
				player.RunVector = new Vector2(0, 0);

			}
			_timerCounter = 0;
			_gameTimer = new Timer(Constants.FPS);
			_gameTimer.AutoReset = true;
			_gameTimer.Elapsed += TimerElapsedListener;
			_gameTimer.Start();
			
        }

		public bool AddPlayer(MainSkyShootService player)
		{
			if (Players.Count >= LocalGameDescription.MaximumPlayersAllowed || IsStarted)
				return false;

			Players.Add(player);
			LocalGameDescription.Players.Add(player.Name);
			if( NewPlayerConnected != null)	NewPlayerConnected(player);
			StartGame += player.GameStart;
			NewPlayerConnected += player.NewPlayerConnected;

			if (Players.Count >= LocalGameDescription.MaximumPlayersAllowed)
			{
                Start();
			}
			return true;
		}

		private void TimerElapsedListener(object sender, EventArgs e)
		{
			if (!IsStarted && StartGame != null)
			{
				StartGame(Players.ToArray(), _gameLevel);
				IsStarted = true;
			}
			update();
			
		}
	#region local functions
		private void SpawnMob()
        {
            if (_intervalToSpawn == 0)
            {
				_intervalToSpawn = (long) Math.Exp(4.8 - _timerCounter/40000)+3;
                
                var mob = _spiderFactory.CreateMob();
                /* 
                 * Все желающие могут убедиться, что сервер посылает разные ID мобов.
                 * issue 10
                 */
                System.Diagnostics.Trace.WriteLine("MobID: " + mob.Id);

                _mobs.Add(mob);
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

            foreach(Mob mob in _mobs)
            {
                mob.Think(_timerCounter, Players);
                mob.Coordinates = ComputeMovement(mob);
            }

            foreach (MainSkyShootService player in Players)
            {
                player.Coordinates = ComputeMovement(player);

                //Проверка на касание игрока и моба
                hitTestTouch(player);
            }

            foreach (AProjectile projectile in _projectiles)
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
			_projectiles.RemoveAll(x => (x.LifeTime <= 0));

			if (_timerCounter % 60 == 0)
			{
				var allObjects = new List<AMob>(_mobs);
				allObjects.AddRange(Players);
				foreach (MainSkyShootService player in Players)
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
         
            foreach(Mob mob in _mobs)
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
            foreach (Mob mob in _mobs)
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

        private Vector2 ComputeMovement(AMob mob)
		{
			var realHeight=_gameLevel.levelHeight-mob.Radius;
			var realWidth=_gameLevel.levelWidth-mob.Radius;
			var newCoord = new Vector2(mob.Coordinates.X + mob.Speed * Constants.FPS * mob.RunVector.X,
				mob.Coordinates.Y + mob.Speed * Constants.FPS * mob.RunVector.Y);
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
	#endregion
	}
}