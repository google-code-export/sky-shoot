/***************************************************************
*  Этот файл был сгенерирован.
*  Не пытайтесь его изменить - он будет перегенерирован заново.
*  Лучше измените файл Weapons.tt
***************************************************************/
using System;

namespace TestArea
{
	class Pistol : AWeapon
	{
		public override Int32 CountBullets { get { return 1; } }

	}

	class Shotgun : AWeapon
	{
		public override String TexturePath { get { return "Shotgun Texture"; } }

		public override Int32 CountBullets { get { return 5; } }

	}

	class Railgun : AWeapon
	{
		public override String TexturePath { get { return "Railgun Texture"; } }

	}

	class Toxicgun : AWeapon
	{
		public override String TexturePath { get { return "Toxic Gun Texture"; } }

		public override Int32 CountBullets { get { return 10; } }

	}

}
