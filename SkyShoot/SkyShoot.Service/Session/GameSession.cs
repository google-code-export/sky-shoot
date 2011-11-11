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
		private GameLevel _gameLevel;
		public List<AMob> players{get; set;}
        public List<AMob> mobs{get; set;}
        public List<AProjectile> projectiles { get; set; }
        private int _movementTime;
        private SpiderFactory _spiderFactory;

		public GameDescription LocalGameDescription { get; private set; }

		public GameSession(TileSet tileSet, List<MainSkyShootService> clients, int maxPlayersAllowed, GameMode gameType, int gameID)
        {
            _movementTime = 1000 / 60;

			_gameLevel = new GameLevel(tileSet);

            players = new List<AMob>();
            players.AddRange(clients.ToArray()); // здесь возможен exception

            var playerNames = new List<string>();

            foreach (MainSkyShootService player in clients)
            {
                this.SomebodyMoves += new SomebodyMovesHadler(player.MobMoved);
                player.MeMoved += new SomebodyMovesHadler(SomebodyMoved);

                this.SomebodyShoots += new SomebodyShootsHandler(player.MobShot);
                player.MeShot += new ClientShootsHandler(SomebodyShot);

                playerNames.Add(player.Name);
            }

			LocalGameDescription = new GameDescription(playerNames, maxPlayersAllowed, gameType, gameID);
            _spiderFactory= new SpiderFactory(_gameLevel);
        }

        public event SomebodyMovesHadler SomebodyMoves;
        public event SomebodyShootsHandler SomebodyShoots;

        public void SomebodyMoved(AMob sender, Vector2 direction)
        {
            if (SomebodyMoves != null)
            {
                SomebodyMoves(sender, direction);
            }
        }

        public void SomebodyShot(AMob sender, Vector2 direction)
        {
            if (SomebodyShoots != null)
            {
                if ((sender as MainSkyShootService).Weapon != null)
                    SomebodyShoots(sender, 
                        (sender as MainSkyShootService).Weapon.CreateBullets(sender, direction));
            }
        }

        // здесь будут производится обработка всех действий
        public void update() 
        {
            var begin = System.DateTime.Now.Ticks * 10000;              

            foreach(AMob mob in mobs){
                // @TODO АИ мобов 
                mob.Coordinates = ComputeMovement(mob);
            }
            foreach(AMob player in players)
                player.Coordinates = ComputeMovement(player);

            //@TODO collision detection;
            
            foreach (AProjectile projectile in projectiles)
            {
                projectile.Coordinates += projectile.Direction*projectile.Speed;
                projectile.Timer--;
                
            }
            projectiles.RemoveAll(x=>(x.Timer<=0));

            var end = System.DateTime.Now.Ticks * 10000;

            var a = new Timer((double) Math.Max(1,_movementTime-(end-begin)));
            a.Elapsed +=new ElapsedEventHandler(TimerElapsedListener);
        }

        private void TimerElapsedListener(object sender,EventArgs e)
        {
            update();
        }

        public Vector2 ComputeMovement(AMob mob)
		{
			var realHeight=_gameLevel.levelHeight-mob.Radius;
			var realWidth=_gameLevel.levelWidth-mob.Radius;
			var newCoord = new Vector2(mob.Coordinates.X + mob.Speed * _movementTime * mob.RunVector.X, mob.Coordinates.Y + mob.Speed * _movementTime * mob.RunVector.Y);
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