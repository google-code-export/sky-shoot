using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Mobs;
using Microsoft.Xna.Framework;
using SkyShoot.Contracts.Service;

namespace SkyShoot.Service.Session
{
    

    public class GameSession
    {
		private GameLevel _gameLevel;
		private List<AMob> Mobs;

		public GameDescription LocalGameDescription { get; private set; }

		public GameSession(TileSet tileSet, List<MainSkyShootService> players, int maxPlayersAllowed, GameMode gameType, int gameID)
        {
			_gameLevel = new GameLevel(tileSet);

            Mobs = new List<AMob>();
            Mobs.AddRange(players.ToArray()); // здесь возможен exception

            foreach (MainSkyShootService player in players)
            {
                this.SomebodyMoves += new SomebodyMovesHadler(player.MobMoved);
                player.MeMoved += new SomebodyMovesHadler(SomebodyMoved);
            }

//			LocalGameDescription = new GameDescription(players, maxPlayersAllowed, gameType, gameID);
            // @todo: получить список имен из players
        }

        public event SomebodyMovesHadler SomebodyMoves;

        public void SomebodyMoved(AMob sender, Vector2 direction)
        {
            if (SomebodyMoves != null)
            {
                SomebodyMoves(sender, direction);
            }
        }

    }
}