using System;
using System.Runtime.Serialization;

namespace SkyShoot.Contracts.Weapon
{
    [Flags]
	public enum WeaponType
	{
		[EnumMember]
		Pistol,

		[EnumMember]
		Shotgun,

		[EnumMember]
		RocketPistol,

		[EnumMember]
		SpiderPistol,

		[EnumMember]
		Heater,

		[EnumMember]
		FlamePistol,

		[EnumMember]
		MobGenerator,

		[EnumMember]
		PoisonGun,

		[EnumMember]
		PoisonTick
	}
}