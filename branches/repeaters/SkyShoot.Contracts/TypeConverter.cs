using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Contracts
{
	public static class GameObjectConverter
	{
		public static AGameObject OneObject(AGameObject m)
		{
			return new AGameObject(m);
		}

		public static AGameObject[] ArrayedObjects(AGameObject[] ms)
		{
			var rs = new AGameObject[ms.Length];
			for (int i = 0; i < ms.Length; i++)
			{
				rs[i] = new AGameObject(ms[i]);
			}
			return rs;
		}

		//public static AProjectile Projectile(AProjectile p)
		//{
		//  return new AProjectile(p);
		//}

		//public static AProjectile[] Projectiles(AProjectile[] ps)
		//{
		//  var rs = new AProjectile[ps.Length];
		//  for (int i = 0; i < ps.Length; i++)
		//  {
		//    rs[i] = new AProjectile(ps[i]);
		//  }
		//  return rs;
		//}
	}
}
