using System;
using System.Runtime.Serialization;
using SkyShoot.Contracts.Mobs;

using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.Bonuses
{
	[DataContract]
	public class AGameBonus : AGameObject
	{
		protected int milliseconds; // @Sergey Terechenko : time = health
		protected long startTime;

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
			milliseconds = b.milliseconds;
			startTime = b.startTime;
		}

		public bool IsExpired(long time)
		{
			return (time - startTime > milliseconds);
		}

		public void Taken(long startTime)
		{
			startTime = startTime;
		}
	}               
}
