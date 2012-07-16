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
		}

		public Team(int newNumber)
		{
			Number = newNumber;
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
