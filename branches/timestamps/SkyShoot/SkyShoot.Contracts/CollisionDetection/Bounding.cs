using System.Runtime.Serialization;

namespace SkyShoot.Contracts.CollisionDetection
{
	[DataContract]
	[KnownType(typeof(BoundingCircle))]
	[KnownType(typeof(BoundingRectangle))]
	public class Bounding
	{
		[DataMember]
		public float Radius { get; set; }
		[DataMember]
		public bool IsRectangle { get; protected set; }
	}
}
