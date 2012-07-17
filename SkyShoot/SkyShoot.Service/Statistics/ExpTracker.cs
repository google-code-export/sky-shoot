﻿using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Statistics;

namespace SkyShoot.Service.Statistics
{
	public abstract class ExpTracker
	{
		protected Stats Value;

		protected ExpTracker()
		{
			Value = new Stats { Exp = 0, Frag = 0, Lvl = 1 };
		}

		protected ExpTracker(Stats st)
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