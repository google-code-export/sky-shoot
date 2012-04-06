using System;

using System.Runtime.Serialization;

using SkyShoot.XNA.Framework;

using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Contracts.Weapon.Projectiles
{
	[DataContract]
	public class AProjectile : AGameObject
	{
		protected AProjectile(AGameObject owner, Guid id, Vector2 direction)
		{
			Owner = owner;
			Id = id;
			RunVector = direction;
			Coordinates = owner.Coordinates;
		}

		public AProjectile()
		{
			Owner = null;
		}

		public override void Copy(AGameObject other)
		{
			// todo temporary
			/*if (!(other is AProjectile))
			{
				throw new Exception("Type mistmath");
			}*/
			// todo otherProjectile == null!
			// var otherProjectile = other as AProjectile;
			base.Copy(other);
			//LifeDistance = otherProjectile.LifeDistance;
			// todo refactor
			// Owner = otherProjectile.Owner;
		}

		public AProjectile(AGameObject other)
		{
			// todo temporary
			// if (!(other is AProjectile))
			// {
			//	throw new Exception("Type mistmath");
			// }
			Copy(other);
		}

		public AGameObject Owner { get; set; }

		//[Obsolete("Use health")]
		//public float LifeDistance { get; set; }

		public override void Do(AGameObject obj, long time)
		{
			if(Owner.Id == obj.Id) // не трогать создателя пули
				return;
			obj.HealthAmount -= Damage;
			// убираем пулю
			IsActive = false;
		}

		public override Vector2 ComputeMovement(long updateDelay, Session.GameLevel gameLevel)
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
