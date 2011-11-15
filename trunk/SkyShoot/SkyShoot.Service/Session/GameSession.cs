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

		public GameSession(TileSet tileSet, int maxPlayersAllowed, GameMode gameType, int gameID)
        {
            IsStarted = false;
			_gameLevel = new GameLevel(tileSet);

            players = new List<MainSkyShootService>();

            var playerNames = new List<string>();

			LocalGameDescription = new GameDescription(playerNames, maxPlayersAllowed, gameType, gameID);
            _spiderFactory= new SpiderFactory(_gameLevel);
        }

        public event SomebodyMovesHadler SomebodyMoves;
        public event SomebodyShootsHandler SomebodyShoots;
        public event StartGameHandler StartGame;

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

        //фунция для спавна мобов
        public bool SpawnMob()
        {
            if (_intervalToSpawn == 0)
            {
                _intervalToSpawn = (long) Math.Exp(4.8 - _timerCounter/40000)+3;
                return true;
            }
            else
            {
                _intervalToSpawn--;
                return false;
            }
        }

        // здесь будут производится обработка всех действий
        private void update() 
        {
            if (SpawnMob())
            {
                mobs.Add(_spiderFactory.CreateMob());
            }

            if (_timerCounter % 6 == 0)// раз в 6 тиков(0.1 секунды)
            {
                foreach(Mob mob in mobs)
                {
                    if (!players.Contains(mob.targetPlayer)) // проверяем цель
                    {
                        mob.FindTarget(players);
                    }
                    mob.Move();
                    mob.Coordinates = ComputeMovement(mob);
                }
            }

            foreach(AMob player in players)
                player.Coordinates = ComputeMovement(player);

            //@TODO collision detection;
            
            foreach (AProjectile projectile in projectiles)
            {
                projectile.Coordinates += projectile.Direction*projectile.Speed;
                projectile.Timer--;
                
            }
			projectiles.RemoveAll(x => (x.Timer <= 0));
			_timerCounter++;
			if (_timerCounter % 60 == 0 )
				foreach (MainSkyShootService player in players)
				{
					player.SynchroFrame(mobs.ToArray());
				}

        }

        public bool Start()
        {
			if (IsStarted) return false;
            IsStarted = true;

			foreach (MainSkyShootService player in players)
			{
				this.SomebodyMoves += new SomebodyMovesHadler(player.MobMoved);
				player.MeMoved += new SomebodyMovesHadler(SomebodyMoved);

				this.SomebodyShoots += new SomebodyShootsHandler(player.MobShot);
				player.MeShot += new ClientShootsHandler(SomebodyShot);

				
			}

            _gameTimer = new Timer(FPS);
            _gameTimer.AutoReset = true;
            _gameTimer.Elapsed += new ElapsedEventHandler(TimerElapsedListener);
			_gameTimer.Start();
            _timerCounter = 1;

            return true;
        }

		public bool AddPlayer(MainSkyShootService player)
		{
			if (players.Count >= LocalGameDescription.MaximumPlayersAllowed)
				return false;

			players.Add(player);
			LocalGameDescription.Players.Add(player.Name);
			StartGame += new StartGameHandler(player.GameStart);
			if (players.Count >= LocalGameDescription.MaximumPlayersAllowed)
			{
				StartGame(mobs.ToArray(), _gameLevel);
				Start();
			}
			return true;
		}

        private void TimerElapsedListener(object sender,EventArgs e)
        {
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