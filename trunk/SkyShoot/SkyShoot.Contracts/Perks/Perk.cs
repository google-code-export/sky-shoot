using System;
using System.Runtime.Serialization;
using SkyShoot.Contracts.GameObject;

namespace SkyShoot.Contracts.Perks
{
	[DataContract]
	public abstract class Perk
	{
		[Flags]
		public enum Perks
		{
			[EnumMember]
			HighSpeed,
			[EnumMember]
			FireCough,
			[EnumMember]
			Regeneration
		}

		[DataMember]
		public Guid Id { get; set; }

		public AGameObject Owner { get; set; }

		protected Perk(Guid id)
		{
			Id = id;
		}

		protected Perk()
		{

		}
	}
}