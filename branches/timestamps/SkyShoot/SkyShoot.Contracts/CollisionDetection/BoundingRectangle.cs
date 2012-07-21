using System.Runtime.Serialization;

namespace SkyShoot.Contracts.CollisionDetection
{
	[DataContract]
	public class BoundingRectangle : Bounding
	{
		[DataMember]
		public float Width { get; set; }
		[DataMember]
		public float Height { get; set; }

		public BoundingRectangle()
		{
			IsRectangle = true;
			Width = Height = 0f;
		}

		public BoundingRectangle(float width, float height)
		{
			IsRectangle = true;
			Width = width;
			Height = height;
		}
	}
}
