namespace SkyShoot.Contracts.CollisionDetection
{
	public class BoundingRectangle : Bounding
	{
		public float Width { get; set; }
		public float Height { get; set; }

		public BoundingRectangle()
		{
			Width = Height = 0f;
		}

		public BoundingRectangle(float width, float height)
		{
			Width = width;
			Height = height;
		}
	}
}
