using System.Runtime.Serialization;

namespace SkyShoot.Contracts.CollisionDetection
{
	[DataContract]
	public class BoundingCircle : Bounding
	{
		public BoundingCircle()
		{
			IsRectangle = false;
		}
	}
}
