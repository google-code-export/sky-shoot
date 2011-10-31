using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Service.Session
{
    public class GameSession
    {
		private GameLevel GameLevel;
		private List<AMob> Mobs;

		public GameDescription LocalGameDescription { get; private set; }

		public GameSession(TileSet tileSet,List<string> players, int maxPlayersAllowed, GameMode gameType, int gameID)
        {
			GameLevel = new GameLevel(tileSet);

			LocalGameDescription = new GameDescription(players, maxPlayersAllowed, gameType, gameID);

        }

    }
}