using System;

using System.Runtime.Serialization;

using Microsoft.Xna.Framework;

using SkyShoot.Contracts.Bonuses;

namespace SkyShoot.Contracts.Mobs
{
	[DataContract]
	public class AMob
	{

		[DataMember]
		public bool IsPlayer { get; set; }

		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public Vector2 RunVector;// { get; set; }

		[DataMember]
		public Vector2 ShootVector;// { get; set; }

		[DataMember]
		public Vector2 Coordinates;

		[DataMember]
		public float HealthAmount { get; set; }

		[DataMember]
		public float Radius { get; set; } // размер моба

		[DataMember]
		public float Speed { get; set; } //скорость: пикселы в миллисекунду

		[DataMember]
		public AObtainableDamageModifier.AObtainableDamageModifiers State { get; set; }

		public AMob(Vector2 coordinates, Guid id)
		{
			RunVector = ShootVector = new Vector2(0, 1);
			Coordinates = coordinates;
			Id = id;
		}

		public AMob(AMob other)
		{
			Copy(other);
		}

		public void Copy(AMob other)
		{
			Coordinates = other.Coordinates;
			Id = other.Id;
			HealthAmount = other.HealthAmount;
			IsPlayer = other.IsPlayer;
			Radius = other.Radius;
			RunVector = other.RunVector;
			ShootVector = other.ShootVector;
			Speed = other.Speed;
			State = other.State;
		}

		public AMob()
		{
		}

		// расширить типом моба, размером, цветом, и т.д.
	}
}
