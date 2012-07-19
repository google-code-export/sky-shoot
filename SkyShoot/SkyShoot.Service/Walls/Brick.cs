using System;
using SkyShoot.Contracts.GameObject;
using SkyShoot.XNA.Framework;
using SkyShoot.Contracts.CollisionDetection;

namespace SkyShoot.Contracts.Mobs
{
	public class Brick : Wall
	{
		public Brick(Vector2 coorinates, float width, float height, Vector2 direction, Guid id)
			: base(coorinates, 0, id)
		{
			ObjectType = EnumObjectType.Brick;
			//ObjectType = EnumObjectType.Wall;
			Bounding = new BoundingRectangle(width, height);
			Radius = (float)Math.Sqrt(width*width + height*height);
			ShootVector = RunVector = direction;
		}

		public override Vector2 ComputeMovement(long updateDelay, Session.GameLevel gameLevel)
		{
			return Coordinates;
		}
	}
}
