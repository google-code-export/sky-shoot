using System;
using System.Collections.Generic;
using SkyShoot.Contracts;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Service.Mobs
{
	class SpiderWithSimpleMind : Spider
	{
		public SpiderWithSimpleMind(float healthAmount)
			: base(healthAmount)
		{
		}

		public override IEnumerable<AGameEvent> Think(List<AGameObject> gameObjects, List<AGameObject> newGameObjects, long time)
		{
			var r = new List<AGameEvent>(base.Think(gameObjects, newGameObjects, time));
			if (RunVector.LengthSquared() < Constants.Epsilon && Wait < 1)//&& PrevMoveDiff.LengthSquared() < Constants.Epsilon)
			{
				//the object is trying to move, but it can't
				// i.e a wall or another mob exist on the path to <b>Target</b>
				var l = new List<AGameObject>();
				AGameObject nearest = null;
				// set to the max distance on the game
				float minLen = Constants.LEVELBORDER * Constants.LEVELBORDER,
					len;
				for (int i = 0; i < gameObjects.Count; i++)
				{
					var p = gameObjects[i];
					// myself
					if (Id == p.Id || p.Is(EnumObjectType.Bonus) || p.Is(EnumObjectType.Bullet))
						continue;
					len = (p.Coordinates - Coordinates).Length();
					if (len < 4 * (p.Radius + Radius) )
					{
						l.Add(p);
					}
					else
					{
						// no sence to look at far objects
						continue;
					}
					if (len < minLen)
					{
						minLen = len;
						nearest = p;
					}
				}
				if (nearest != null && Target != null && nearest != Target)
				{
					var toObject = nearest.Coordinates - Coordinates;
					var toTarget = Target.Coordinates - Coordinates;
					toObject.Normalize();
					toTarget.Normalize();
					ThinkCounter = 0;
					//!! evristic
					Wait = (int)Math.Floor(Radius * 1f);
					//var angle = Math.Acos(Vector2.Dot(toOnject, toTarget));
					//if(angle < 0)
					//{
					//  RunVector = new Vector2(RunVector.Y, -RunVector.X);
					//}
					//else
					//{
					//  RunVector = new Vector2(-RunVector.Y, RunVector.X);
					//}
					RunVector = toTarget - toObject;
					RunVector.Normalize();
				}

			}
			return r;
		}
	}
}

