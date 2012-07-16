using System;
using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Bonuses
{
	public class AGameBonus : AGameObject
	{
		protected int Milliseconds; // @Sergey Terechenko : time = health
		protected long StartTime;

		public float DamageFactor;

		public AGameBonus(Vector2 coordinates)
			: base(coordinates, Guid.NewGuid())
		{
			// the object is alive
			HealthAmount = MaxHealthAmount = 1;
			Radius = 6;
		}

		public AGameBonus()
		{
			HealthAmount = MaxHealthAmount = 1;
			Radius = 6;
		}

		public override void Copy(AGameObject other)
		{
			base.Copy(other);
			var b = other as AGameBonus;
			if (b == null)
				return; // throw
			Milliseconds = b.Milliseconds;
			StartTime = b.StartTime;
			DamageFactor = b.DamageFactor;
		}

		public bool IsExpired(long time)
		{
			return (time - StartTime > Milliseconds);
		}

		public void Taken(long startTime)
		{
			StartTime = startTime;
		}
	}
}
