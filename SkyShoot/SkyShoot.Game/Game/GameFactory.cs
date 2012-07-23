using SkyShoot.Contracts.GameObject;
using SkyShoot.Game.View;

namespace SkyShoot.Game.Game
{
	internal class GameFactory
	{
		public static DrawableGameObject CreateClientGameObject(AGameObject serverGameObject)
		{
			switch (serverGameObject.ObjectType)
			{
				case AGameObject.EnumObjectType.Player:
					return new DrawableGameObject(serverGameObject, Textures.PlayerAnimation);
				case AGameObject.EnumObjectType.Turret:
					return new DrawableGameObject(serverGameObject, Textures.Turret);
				case AGameObject.EnumObjectType.TurretGunBullet:
					return new DrawableGameObject(serverGameObject, Textures.TurretProjectile);
				case AGameObject.EnumObjectType.Mob:
					return new DrawableGameObject(serverGameObject, Textures.SpiderAnimation);
				case AGameObject.EnumObjectType.Flame:
					return new DrawableGameObject(serverGameObject, Textures.FlameProjectile);
				case AGameObject.EnumObjectType.PistolBullet:
					return new DrawableGameObject(serverGameObject, Textures.LaserProjectile);
				case AGameObject.EnumObjectType.HeaterBullet:
					return new DrawableGameObject(serverGameObject, Textures.HeaterProjectile);
				case AGameObject.EnumObjectType.ShotgunBullet:
					return new DrawableGameObject(serverGameObject, Textures.ShotgunProjectile);
				case AGameObject.EnumObjectType.RocketBullet:
					return new DrawableGameObject(serverGameObject, Textures.RocketProjectile);
				case AGameObject.EnumObjectType.Explosion:
					return new DrawableGameObject(serverGameObject, Textures.Explosion);
				case AGameObject.EnumObjectType.SpiderBullet:
					return new DrawableGameObject(serverGameObject, Textures.SpiderProjectile); //TODO: SPIDER!!!
				case AGameObject.EnumObjectType.DoubleDamage:
					return new DrawableGameObject(serverGameObject, Textures.DoubleDamage);
				case AGameObject.EnumObjectType.Shield:
					return new DrawableGameObject(serverGameObject, Textures.Protection);
				case AGameObject.EnumObjectType.Remedy:
					return new DrawableGameObject(serverGameObject, Textures.MedChest);
				case AGameObject.EnumObjectType.Speedup:
					return new DrawableGameObject(serverGameObject, Textures.Speed);
				case AGameObject.EnumObjectType.Mirror:
					return new DrawableGameObject(serverGameObject, Textures.Mirror);
				case AGameObject.EnumObjectType.Wall:
					return new DrawableGameObject(serverGameObject, Textures.OneStone);
				case AGameObject.EnumObjectType.Brick:
					return new DrawableGameObject(serverGameObject, Textures.Brick);
				case AGameObject.EnumObjectType.PoisonBullet:
					return new DrawableGameObject(serverGameObject, Textures.PoisonProjectile);
				case AGameObject.EnumObjectType.Poisoner:
					return new DrawableGameObject(serverGameObject, Textures.Poisoner);
				//case AGameObject.EnumObjectType.Poisoning:
				//	return new DrawableGameObject(serverGameObject, Textures.Poisoning);
				case AGameObject.EnumObjectType.Hydra:
					return new DrawableGameObject(serverGameObject, Textures.Hydra);
				case AGameObject.EnumObjectType.ParentMob:
					return new DrawableGameObject(serverGameObject, Textures.ParentMob);
				case AGameObject.EnumObjectType.Caterpillar:
					return new DrawableGameObject(serverGameObject, Textures.Caterpillar);

				default:
					return new DrawableGameObject(serverGameObject, Textures.Cross);
			}
		}

		public static GameLevel CreateClientGameLevel(Contracts.Session.GameLevel gameLevel)
		{
			return new GameLevel(gameLevel.Width, gameLevel.Height, gameLevel.UsedTileSet);
		}
	}
}