using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Mobs;
using Microsoft.Xna.Framework;

namespace SkyShoot.Service.Session
{
    public delegate void SomebodyMovesHadler(AMob sender, Vector2 direction);

    public class GameSession
    {
		private GameLevel _gameLevel;
		private List<AMob> Mobs;

		public GameDescription LocalGameDescription { get; private set; }

		public GameSession(TileSet tileSet, List<Client.Client> players, int maxPlayersAllowed, GameMode gameType, int gameID)
        {
			_gameLevel = new GameLevel(tileSet);

            foreach (Client.Client player in players)
            {
                this.SomebodyMoves += new SomebodyMovesHadler(player.MobMoved);
            }

//			LocalGameDescription = new GameDescription(players, maxPlayersAllowed, gameType, gameID);
            // @todo: получить список имен из players
        }

 
        public event SomebodyMovesHadler SomebodyMoves;

        public void Move(AMob sender, Vector2 direction)
        {
            if (SomebodyMoves != null)
            {
                SomebodyMoves(sender, direction);
            }
        }

    }
}