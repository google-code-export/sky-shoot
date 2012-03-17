using System.Runtime.Serialization;

namespace SkyShoot.Contracts.Session
{
	[DataContract]
	public enum TileSet
	{
		[EnumMember]
		Snow,
		[EnumMember]
		Desert,
		[EnumMember]
		Grass,
		[EnumMember]
		Sand,
		[EnumMember]
		Volcanic
	}
}
