using System.Runtime.Serialization;

namespace SkyShoot.Contracts.Session
{
	[DataContract]
	public enum GameMode
	{
		[EnumMember]
		Coop,
		//reserved
		[EnumMember]
		Deathmatch,
		//reserved
		[EnumMember]
		Campaign,
	}
}