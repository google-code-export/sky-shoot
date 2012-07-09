using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyShoot.Contracts.Service.Commands
{
	class Player
	{
		public int command;
		public string name;
	}

	class PlayerList
	{
		public List<Player> Players = new List<Player>();

		public void AddPlayer(string name)
		{
			Player newPlayer = new Player();
			newPlayer.name = name;
			newPlayer.command = 1; //По умолчанию
			Players.Add(newPlayer);
		}

		public void AddPlayer(Player player)
		{
			Players.Add(player);
		}

		private Player FindPlayer(string name)
		{
			var found = Players.Find(p => p.name.Equals(name));
			return found;
		}

		public void RemovePlayer(string name)
		{
			Players.Remove(FindPlayer(name));
		}
		
		public void ChangePlayerTeam(string name)
		{
			FindPlayer(name).name = name;
		}

		public int PlayerTeam(string name)
		{
			return FindPlayer(name).command;
		}
	}
}

