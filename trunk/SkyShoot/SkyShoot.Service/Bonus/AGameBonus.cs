using System;
using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Bonuses
{
	//[DataContract]
	public class AGameBonus : AGameObject
	{
		protected int Milliseconds; // @Sergey Terechenko : time = health
		protected long StartTime;

		public float DamageFactor;

		public AGameBonus(Vector2 coordinates) : base(coordinates, Guid.NewGuid()) { }

		public AGameBonus(AGameObject o)
		{
			var b = o as AGameBonus;
			if(b == null)
			{
				throw new TypeAccessException();
			}
			Copy(b);
			Milliseconds = b.Milliseconds;
			StartTime = b.StartTime;
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
