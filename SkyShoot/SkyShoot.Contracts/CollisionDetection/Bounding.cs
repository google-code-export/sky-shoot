using System.Runtime.Serialization;

namespace SkyShoot.Contracts.CollisionDetection
{
	[DataContract]
	[KnownType(typeof(BoundingCircle))]
	[KnownType(typeof(BoundingRectangle))]
	public class Bounding
	{
		public float Radius { get; set; }
		public bool IsRectangle { get; protected set; }
	}
}
