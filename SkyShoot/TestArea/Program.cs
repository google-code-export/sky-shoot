using System;
using System.Linq;
using System.Reflection;

namespace TestArea
{
	class Program
	{
		static void Main()
		{
			Console.WriteLine("Weapons:");

			var types = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.BaseType == typeof(AWeapon));

			foreach (Type type in types)
			{
				Console.WriteLine("----------------------------------------------------");
				var weapon = Activator.CreateInstance(type) as IWeapon;
				Console.WriteLine(" Weapon name: " + weapon.GetType().Name);
				Console.WriteLine(" Weapon texture path: " + weapon.TexturePath);
				Console.WriteLine(" Count bullets: " + weapon.CountBullets);
				Console.WriteLine("----------------------------------------------------");
			}
		}
	}
}