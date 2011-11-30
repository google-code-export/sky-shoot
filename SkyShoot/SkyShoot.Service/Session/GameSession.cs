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
using System.Diagnostics;

namespace SkyShoot.Service.Session
{
	
    public class GameSession
    {
		public List<MainSkyShootService> Players { get; set; }

        private List<Mob> _mobs { get; set; }
        private List<AProjectile> _projectiles { get; set; }

        public GameDescription LocalGameDescription { get; private set; }

        public bool IsStarted { get; set; }		
        private GameLevel _gameLevel;
        private SpiderFactory _spiderFactory;
        private long _timerCounter;
        private long _intervalToSpawn = 0;

        private long _lastUpdate;
        private long _updateDelay;

		private Timer _gameTimer;
        private object _updating;

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
					_projectiles.AddRange(a);
					//Trace.WriteLine("projectile added", "GameSession");
			
					SomebodyShoots(sender, a);
					
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
			this.StartGame -= player.GameStart;
			
            player.MeMoved -= SomebodyMoved;
            player.MeShot -= SomebodyShot;
            //player.MobSpawned -= SomebodySpawned;
            //player.MobDied -= SomebodyDied;

			Players.Remove(player);
            Trace.WriteLine(player.Name + "leaved game");
			
		}

		public void PlayerDead(MainSkyShootService player)
		{

			player.GameOver();
			SomebodyDied(player);            
			player.Disconnect();//временно
		}
		public void Stop()
		{
			if (_gameTimer != null)
			{
				_gameTimer.Enabled = false;
				_gameTimer.AutoReset = false;
				_gameTimer.Elapsed -= TimerElapsedListener;
			}
			IsStarted = false;
		}

        public void Start()
        {
			for(int i = 0; i < Players.Count; i++)
            {
				var player = Players[i];
				this.SomebodyMoves += player.MobMoved;
				player.MeMoved += SomebodyMoved;

				this.SomebodyShoots += player.MobShot;
				player.MeShot += SomebodyShot;

                this.SomebodySpawns += player.SpawnMob;

                this.SomebodyDies += player.MobDead;
                
				this.SomebodyHit += player.Hit;

				player.Coordinates = new Vector2(500,500);
                player.Speed = Constants.PLAYER_DEFAULT_SPEED;
                player.Radius = Constants.PLAYER_RADIUS;
                player.Weapon = new Weapon.Pistol(Guid.NewGuid(), player);
				player.RunVector = new Vector2(0, 0);
				player.HealthAmount = 100;

			}
			System.Threading.Thread.Sleep(1000);
			if (!IsStarted && StartGame != null)
			{
				IsStarted = true;
				StartGame(Players.ToArray(), _gameLevel);
			
			}
			_timerCounter = 0;
			_updating = false;

			_lastUpdate = DateTime.Now.Ticks/10000;
			_updateDelay = 0;
			_gameTimer = new Timer(Constants.FPS);
			_gameTimer.AutoReset = true;
			_gameTimer.Elapsed += TimerElapsedListener;
			_gameTimer.Start();
			Trace.WriteLine("Game Started");
			
        }

		public bool AddPlayer(MainSkyShootService player)
		{
			if (Players.Count >= LocalGameDescription.MaximumPlayersAllowed || IsStarted)
				return false;

			Players.Add(player);
			LocalGameDescription.Players.Add(player.Name);

			if( NewPlayerConnected != null)	NewPlayerConnected(player);

			StartGame += player.GameStart;

			if (Players.Count == LocalGameDescription.MaximumPlayersAllowed)
			{
				Trace.WriteLine("player added"+player.Name);
				var startThread = new System.Threading.Thread(new System.Threading.ThreadStart(Start));
				startThread.Start();
				
                
			}
			return true;
		}

		private void TimerElapsedListener(object sender, EventArgs e)
		{			
			update();
					
		}

	#region local functions
		private void SpawnMob()
        {
            if (_intervalToSpawn == 0)
            {
				_intervalToSpawn = (long) Math.Exp(4.8 - _timerCounter/40000)+3;
                
                var mob = _spiderFactory.CreateMob();
                System.Diagnostics.Trace.WriteLine("mob spawned" + mob.Id);

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
			if (!System.Threading.Monitor.TryEnter(_updating)) return;

			Trace.WriteLine("update begin "+ _timerCounter);
            SpawnMob();
			 var now = DateTime.Now.Ticks/10000;
			_updateDelay = now - _lastUpdate;
			_lastUpdate = now;     

            
			for (int i = 0; i < _mobs.Count; i++)
			{
				var mob = _mobs[i];
                mob.Think(Players);
                mob.Coordinates = ComputeMovement(mob);
                //System.Diagnostics.Trace.WriteLine("Mob cord: " + mob.Coordinates); 

            }

			for(int i = 0; i < Players.Count; i++)
            {
				var player = Players[i];
                player.Coordinates = ComputeMovement(player);

                //Проверка на касание игрока и моба
                hitTestTouch(player);
            }
			
			for(int i = 0; i < _projectiles.Count; i++)
			{
				if (_projectiles[i] == null) continue;
				var projectile = _projectiles[i];
				var newCord = projectile.Coordinates + projectile.Direction * projectile.Speed * _updateDelay;

				//Проверка на касание пули и моба
				var hitedMob = hitTestProjectile(projectile, newCord);
				if (hitedMob == null)
				{
					projectile.OldCoordinates = projectile.Coordinates;
					projectile.Coordinates = newCord;
					projectile.LifeDistance -= Vector2.Distance(projectile.Coordinates, projectile.OldCoordinates);
				}
				else
				{
					hitedMob.DamageTaken(projectile);
					SomebodyHitted(hitedMob, projectile);
					if (hitedMob.HealthAmount <= 0)
					{
						MobDead(hitedMob);
					}
					projectile.LifeDistance = -1;
				}

			}

			_projectiles.RemoveAll(x => (x==null) || (x.LifeDistance <= 0));

			if (_timerCounter % 10 == 0)
			{
				var allObjects = new List<AMob>(_mobs);
				allObjects.AddRange(Players);
				for (int i = 0; i < Players.Count; i++)
				{
					Players[i].SynchroFrame(allObjects.ToArray());
				}
				Trace.WriteLine("SynchroFrame");
			}
			Trace.WriteLine(System.DateTime.Now.Ticks/10000 - now);
			Trace.WriteLine("update end " + _timerCounter);
			_timerCounter++;
			//_updated = false;
			System.Threading.Monitor.Exit(_updating);
        }

        private Mob hitTestProjectile(AProjectile projectile, Vector2 newCord)
        {
            var prX = newCord.X - projectile.Coordinates.X;
            var prY = newCord.Y - projectile.Coordinates.Y;

            Mob hitedTarget = null;
            var minDist = double.MaxValue;

			for (int i = 0; i < _mobs.Count; i++)
			{
				var mob = _mobs[i];
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

                var h = Math.Abs(prX * mY - prY * mX) / Math.Sqrt(prX * prX * + prY * prY);

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
			for (int i = 0; i < _mobs.Count;i++ )
			{
				var mob = _mobs[i];
				if ((mob.Coordinates - player.Coordinates).Length() <= mob.Radius + player.Radius)
				{
					if (mob.Weapon.Reload(System.DateTime.Now.Ticks / 10000))
					{
						player.HealthAmount -= mob.Damage;
						SomebodyHitted(player, null);
					}

					if (player.HealthAmount <= 0)
					{
                        PlayerDead(player);
					}
				}
			}
        } 

        private Vector2 ComputeMovement(AMob mob)
		{
			
			var newCoord = mob.RunVector*mob.Speed*_updateDelay + mob.Coordinates ;

			if (mob.IsPlayer)
			{
				newCoord.X = MathHelper.Clamp(newCoord.X, 0, _gameLevel.levelHeight);
				newCoord.Y = MathHelper.Clamp(newCoord.Y, 0, _gameLevel.levelWidth);
			}
            
			return newCoord;
		}
	#endregion
	}
}