using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Mobs;
using Microsoft.Xna.Framework;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Weapon.Projectiles;

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

                playerNames.Add(player.Name);
            }



			LocalGameDescription = new GameDescription(playerNames, maxPlayersAllowed, gameType, gameID);
            _spiderFactory= new SpiderFactory(_gameLevel, 100, 100, 100); // характеристики моба ИЗМЕНИТЬ!
            
        }

        public event SomebodyMovesHadler SomebodyMoves;

        public void SomebodyMoved(AMob sender, Vector2 direction)
        {
            if (SomebodyMoves != null)
            {
                SomebodyMoves(sender, direction);
            }
        }

        // здесь будут производится обработка всех действий
        public void update() 
        {
            
            foreach(AMob mob in mobs){
                // @TODO АИ мобов 
                mob.Coordinates = ComputeMovement(mob);
            }
            foreach(AMob player in players)
                player.Coordinates = ComputeMovement(player);

            //TODO collision detection;

            foreach (AProjectile projectile in projectiles)
            {
                if (projectile.Timer <= 0) projectiles.Remove(projectile);//тут скорее всего будет ошибка
                projectile.Coordinates += projectile.Direction*projectile.Speed;
                projectile.Timer--;
            }
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