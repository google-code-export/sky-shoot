using System.Windows.Forms;
using SkyShoot.Contracts.Weapon;

namespace SkyShoot.WinFormsClient
{
	class WeaponButton : Label
	{
		public AWeapon.AWeaponType WeaponType { get; set; }
	}
}
