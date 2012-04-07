using System;
using System.Collections.Generic;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.Mobs
{
	public class Wall : AGameObject
	{
		public Wall(Vector2 coorinates, float radius, Guid id):
			base(coorinates, id)
		{
			ObjectType = EnumObjectType.Wall;
			Radius = radius;

			ShootVector = RunVector = Vector2.Zero;
			Speed = 0;
			HealthAmount = 1000000;// очень много, пока хватит
		}

		public override Vector2 ComputeMovement(long updateDelay, Session.GameLevel gameLevel)
		{
			return Coordinates;
		}
	}
}
