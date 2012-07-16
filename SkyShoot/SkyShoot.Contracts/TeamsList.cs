using System.Collections.Generic;

namespace SkyShoot.Contracts
{
	using Mobs;

	public class Team
	{
		public int Number { set; get; }

		public List<AGameObject> Members;

		public Team()
		{
			Number = 0;
			Members = new List<AGameObject>();
		}

		public Team(int newNumber)
		{
			Number = newNumber;
			Members = new List<AGameObject>();
		}
	}

	public class TeamsList
	{
		public List<Team> Teams = new List<Team>();

		public Team GetTeamByNymber(int number)
		{
			Team team = Teams.Find(t => t.Number == number);
			return team;
		}
	}
}
