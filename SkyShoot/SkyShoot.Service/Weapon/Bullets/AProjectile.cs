using System;
using SkyShoot.Contracts.Session;
using SkyShoot.XNA.Framework;
using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Service.Weapon.Bullets
{
	public class AProjectile : AGameObject
	{
		protected AProjectile(AGameObject owner, Guid id, Vector2 direction)
		{
			Owner = owner;
			Id = id;
			ShootVector = RunVector = direction;
			Coordinates = owner.Coordinates;
			Radius = 3;
		}

		public AProjectile()
		{
			Owner = null;
			Radius = 3;
		}

		public override void Copy(AGameObject other)
		{
			if (!(other is AProjectile))
			{
				throw new Exception("Type mistmath");
			}
			var otherProjectile = other as AProjectile;
			base.Copy(other);
			// 'cause health used
			//LifeDistance = otherProjectile.LifeDistance;
			Owner = otherProjectile.Owner;
		}

		//public AProjectile(AGameObject other)
		//{
		//  // if (!(other is AProjectile))
		//  // {
		//  //	throw new Exception("Type mistmath");
		//  // }
		//  Copy(other);
		//}

		public AGameObject Owner { get; set; }

		//[Obsolete("Use health")]
		//public float LifeDistance { get; set; }

		public override void Do(AGameObject obj, long time)
		{
			if (Owner.Id == obj.Id) // не трогать создателя пули
				return;

			if (obj.Is(EnumObjectType.LivingObject))
			{
				//!! todo сделать проверку на бонус "щит" у обжекта
				obj.HealthAmount -= Damage;
				// убираем пулю
				IsActive = false;
			}
			if(obj.Is(EnumObjectType.Wall))
			{
				IsActive = false;
			}
		}

		public override Vector2 ComputeMovement(long updateDelay, GameLevel gameLevel)
		{
			//!! rewrite
			var newCoord = base.ComputeMovement(updateDelay, gameLevel);
			var x = MathHelper.Clamp(newCoord.X, 0, gameLevel.levelHeight);
			var y = MathHelper.Clamp(newCoord.Y, 0, gameLevel.levelWidth);
			const float epsilon = 0.01f;
			// убрать пулю, которая вышла за экран
			IsActive = (Math.Abs(newCoord.X - x) < epsilon) 
				&& (Math.Abs(newCoord.Y - y) < epsilon);
			return newCoord;
		}
	}
}
