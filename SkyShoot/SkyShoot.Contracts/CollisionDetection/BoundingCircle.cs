using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyShoot.Contracts.CollisionDetection
{
	public class BoundingCircle : Bounding
	{
		public float _radius
		{
			get
			{
				return Radius;
			}
			set
			{
				Radius = value;
			}
		}
		public BoundingCircle()
		{
			_radius = Radius;
			IsRectangle = false;
		}
	}
}
