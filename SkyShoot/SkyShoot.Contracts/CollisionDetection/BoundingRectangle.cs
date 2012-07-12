using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyShoot.Contracts.CollisionDetection
{
	public class BoundingRectangle : Bounding
	{
		public float _width { get; set; }
		public float _height { get; set; }
		public BoundingRectangle()
		{
			_width = _height = 0f;
		}
		public BoundingRectangle(float width, float height)
		{
			_width = width;
			_height = height;
		}
	}
}
