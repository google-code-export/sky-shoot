using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Contracts
{
	public static class GameObjectConverter
	{
		public static AGameObject OneObject(AGameObject m)
		{
			var o = new AGameObject();
			o.Copy(m);
			return o;
		}

		public static AGameObject[] ArrayedObjects(AGameObject[] ms)
		{
			var rs = new AGameObject[ms.Length];
			for (int i = 0; i < ms.Length; i++)
			{
				rs[i] = new AGameObject();
				rs[i].Copy(ms[i]);
			}
			return rs;
		}
	}
}
