using SkyShoot.Contracts.GameObject;

namespace SkyShoot.Contracts.Utils
{
	public static class GameObjectConverter
	{
		public static AGameObject OneObject(AGameObject gameObject)
		{
			var gameObjectCopy = new AGameObject();
			gameObjectCopy.Copy(gameObject);
			return gameObjectCopy;
		}

		public static AGameObject[] ArrayedObjects(AGameObject[] gameObjects)
		{
			var result = new AGameObject[gameObjects.Length];
			for (int i = 0; i < gameObjects.Length; i++)
			{
				result[i] = new AGameObject();
				result[i].Copy(gameObjects[i]);
			}
			return result;
		}
	}
}
