using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Statistics;

namespace SkyShoot.Service.Statistics
{
	public abstract class ExperienceTracker
	{
		protected Stats Value;

		protected ExperienceTracker()
		{
			Value = new Stats { Experience = 0, Frag = 0, Level = 1};
		}

		protected ExperienceTracker(Stats st)
		{
			Value = st;
		}

		public Stats GetStats()
		{
			return Value;
		}

		public abstract void AddExpPlayer(AGameObject owner, AGameObject wounded, int damage);
		public abstract void AddExpTeam(AGameObject player, AGameObject wounded, int damage, int teamMembers);
	}
}
