using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyShoot.Contracts
{
	using SkyShoot.Contracts.Mobs;

	public class Team
	{
		public int number { set; get; }

		public List<AGameObject> members;

		public Team()
		{
			number = 0;
		}

		public Team(int newNumber)
		{
			number = newNumber;
		}
	}
	
	public class TeamsList
	{
	public List<Team> Teams = new List<Team>();

		public Team GetTeamByNymber(int number)
		{
			Team team = Teams.Find(t => t.number == number);
			return team;
		}
}
}
