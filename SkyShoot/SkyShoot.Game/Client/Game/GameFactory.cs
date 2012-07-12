using SkyShoot.Contracts.Mobs;
using SkyShoot.Game.Client.GameObjects;
using SkyShoot.Game.Client.View;

namespace SkyShoot.Game.Client.Game
{
	internal class GameFactory
	{
		private static SoundManager _soundManager;

		public static DrawableGameObject CreateClientMob(AGameObject serverGameObject)
		{
			SoundManager.Initialize();
			_soundManager = SoundManager.Instance;

			// can't uhse switch 'cause one object can have many merged EnumTypes
			if (serverGameObject.Is(AGameObject.EnumObjectType.Player))
				return new DrawableGameObject(serverGameObject, Textures.PlayerAnimation);

			if (serverGameObject.Is(AGameObject.EnumObjectType.Mob))
			{
				_soundManager.SoundPlay(SoundManager.SoundEnum.Spider);
				return new DrawableGameObject(serverGameObject, Textures.SpiderAnimation);
			}
			/*if (serverGameObject.Is(AGameObject.EnumObjectType.Bullet))
				return new DrawableGameObject(serverGameObject, Textures.ProjectileTexture);
			
			if (serverGameObject.Is(AGameObject.EnumObjectType.Bonus))
				return new DrawableGameObject(serverGameObject, Textures.Plus);*/

			if (serverGameObject.Is(AGameObject.EnumObjectType.Flame))
				return new DrawableGameObject(serverGameObject, Textures.FlameProjectile);

			if (serverGameObject.Is(AGameObject.EnumObjectType.PistolBullet))
				return new DrawableGameObject(serverGameObject, Textures.LaserProjectile);

			if(serverGameObject.Is(AGameObject.EnumObjectType.HeaterBullet))
				return new DrawableGameObject(serverGameObject, Textures.HeaterProjectile);

			if (serverGameObject.Is(AGameObject.EnumObjectType.ShotgunBullet))
				return new DrawableGameObject(serverGameObject, Textures.ShotgunProjectile);

			if (serverGameObject.Is(AGameObject.EnumObjectType.RocketBullet))
				return new DrawableGameObject(serverGameObject, Textures.RocketProjectile);

			if (serverGameObject.Is(AGameObject.EnumObjectType.PoisonBullet))
				return new DrawableGameObject(serverGameObject, Textures.PoisonTickProjectile);

			if (serverGameObject.Is(AGameObject.EnumObjectType.PoisonTickBullet))
				return new DrawableGameObject(serverGameObject, Textures.PoisonTickProjectile);

			if (serverGameObject.Is(AGameObject.EnumObjectType.Explosion))
				return new DrawableGameObject(serverGameObject, Textures.Explosion);

			if (serverGameObject.Is(AGameObject.EnumObjectType.SpiderBullet))
				return new DrawableGameObject(serverGameObject, Textures.SpiderProjectile);

			if (serverGameObject.Is(AGameObject.EnumObjectType.DoubleDamage))
				return new DrawableGameObject(serverGameObject, Textures.DoubleDamage);

			if (serverGameObject.Is(AGameObject.EnumObjectType.Shield))
				return new DrawableGameObject(serverGameObject, Textures.Protection);

			if (serverGameObject.Is(AGameObject.EnumObjectType.Remedy))
				return new DrawableGameObject(serverGameObject, Textures.MedChest);

			if (serverGameObject.Is(AGameObject.EnumObjectType.Speedup))
				return new DrawableGameObject(serverGameObject, Textures.Speed);

			if(serverGameObject.Is(AGameObject.EnumObjectType.Mirror))
				return new DrawableGameObject(serverGameObject, Textures.Mirror);

			if(serverGameObject.Is(AGameObject.EnumObjectType.Wall))
				return new DrawableGameObject(serverGameObject, Textures.OneStone);

			if (serverGameObject.Is(AGameObject.EnumObjectType.Turret))
				return new DrawableGameObject(serverGameObject, Textures.OneStone); //todo here

			return null;
			//throw new Exception();
		}
		
		//public static Projectile CreateClientProjectile(AGameObject serverGameObject)
		//{
		//  return new Projectile(serverGameObject);
		//}

		//public static GameBonus CreateClientGameBonus(AGameObject serverGameObject)
		//{
		//  return new GameBonus(serverGameObject);
		//}

		public static GameLevel CreateClientGameLevel(Contracts.Session.GameLevel gameLevel)
		{
			return new GameLevel(gameLevel);
		}
	}
}