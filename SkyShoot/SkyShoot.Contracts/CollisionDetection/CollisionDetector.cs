using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Contracts.CollisionDetection
{
	/// <summary>
	/// Class for detect of collisions. Any function named FitObjects return a fit-vector
	/// </summary>
	public class CollisionDetector
	{
		protected static Vector2 ProjectRectangle(Vector2 recPosition, float recWidth, float recHeight, float recRotation, Vector2 ort)
		{
			float[] prPoints = new float[4];

			float min;
			float max;

			Vector2[] objPoints = new Vector2[4];
			objPoints[0].X = -recWidth / 2f;
			objPoints[0].Y = recHeight / 2f;
			objPoints[1].X = recWidth / 2f;
			objPoints[1].Y = recHeight / 2f;
			objPoints[2].X = recWidth / 2f;
			objPoints[2].Y = -recHeight / 2f;
			objPoints[3].X = -recWidth / 2f;
			objPoints[3].Y = -recHeight / 2f;
			Matrix matRotation = new Matrix();
			matRotation = Matrix.CreateRotationZ(recRotation);
			for (int i = 0; i < 4; i++)
				objPoints[i] = Vector2.Transform(objPoints[i], matRotation) + recPosition;

			for (int i = 0; i < 4; i++)
			{
				float dp = Vector2.Dot(objPoints[i], ort);
				prPoints[i] = (dp * ort).Length();
			}

			min = Math.Min(Math.Min(prPoints[0], prPoints[1]), Math.Min(prPoints[2], prPoints[3]));
			max = Math.Max(Math.Max(prPoints[0], prPoints[1]), Math.Max(prPoints[2], prPoints[3]));

			return new Vector2(min, max);
		}

		protected static Vector2 DistanceTo(Vector2 point, Vector2 begin, Vector2 end)
		{
			if (Vector2.Dot(point - begin, end - begin) > 0f && Vector2.Dot(end - point, end - begin) > 0f)
			{
				Vector2 ortoVec = new Vector2((end - begin).Y, -(end - begin).X);
				ortoVec.Normalize();
				float dp = Vector2.Dot(point - begin, ortoVec);
				return -dp * ortoVec;
			}
			if ((point - end).Length() <= (point - begin).Length())
				return -(point - end);
			else
				return -(point - begin);
		}

		/// <summary>
		/// Return false if X or Y component not equal zero
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public static bool Collision(Vector2 vector)
		{
			return vector.X != 0f || vector.Y != 0f;
		}

		/// <summary>
		/// Return fit-vector for collision rectangles
		/// </summary>
		/// <param name="objActivePosition"></param>
		/// <param name="objActiveWidth"></param>
		/// <param name="objActiveHeight"></param>
		/// <param name="objActiveRotation"></param>
		/// <param name="objPassivePosition"></param>
		/// <param name="objPassiveWidth"></param>
		/// <param name="objPassiveHeight"></param>
		/// <param name="objPassiveRotation"></param>
		/// <returns></returns>
		public static Vector2 FitObjects(Vector2 objActivePosition, float objActiveWidth, float objActiveHeight, float objActiveRotation, Vector2 objPassivePosition, float objPassiveWidth, float objPassiveHeight, float objPassiveRotation)
		{
			Vector2[] objActivePoints = new Vector2[2];
			Vector2[] objPassivePoints = new Vector2[2];
			Vector2[] orts = new Vector2[4];
			Matrix matRotation = new Matrix();
			float length = 0;
			int index = 0;
			int intersect = 0;

			objActivePoints[0].X = -objActiveWidth / 2f;
			objActivePoints[0].Y = objActiveHeight / 2f;
			objActivePoints[1].X = objActiveWidth / 2f;
			objActivePoints[1].Y = objActiveHeight / 2f;
			matRotation = Matrix.CreateRotationZ(objActiveRotation);
			for (int i = 0; i < 2; i++)
				objActivePoints[i] = Vector2.TransformNormal(objActivePoints[i], matRotation) + objActivePosition;

			objPassivePoints[0].X = -objPassiveWidth / 2f;
			objPassivePoints[0].Y = objPassiveHeight / 2f;
			objPassivePoints[1].X = objPassiveWidth / 2f;
			objPassivePoints[1].Y = objPassiveHeight / 2f;
			matRotation = Matrix.CreateRotationZ(objPassiveRotation);
			for (int i = 0; i < 2; i++)
				objPassivePoints[i] = Vector2.TransformNormal(objPassivePoints[i], matRotation) + objActivePosition;

			orts[0] = objActivePoints[1] - objActivePoints[0];
			orts[2] = objPassivePoints[1] - objPassivePoints[0];
			orts[1] = new Vector2(orts[0].Y, -orts[0].X);
			orts[3] = new Vector2(orts[2].Y, -orts[2].X);

			for (int i = 0; i < 4; i++)
				orts[i].Normalize();

			for (int i = 0; i < 4; i++)
			{
				Vector2 interval1 = ProjectRectangle(objActivePosition, objActiveWidth, objActiveHeight, objActiveRotation, orts[i]);
				Vector2 interval2 = ProjectRectangle(objPassivePosition, objPassiveWidth, objPassiveHeight, objPassiveRotation, orts[i]);
				if (Math.Max(interval1.Y, interval2.Y) - Math.Min(interval1.X, interval2.X) < interval1.Y - interval1.X + interval2.Y - interval2.X)
				{
					intersect++;
					float tempLen = Math.Min(interval1.Y - interval2.X, interval2.Y - interval1.X);
					if (i == 0)
						length = tempLen;

					if (tempLen <= length)
					{
						length = tempLen;
						index = i;
					}
				}
			}

			if (intersect == 4)
			{
				if (Vector2.Dot(objPassivePosition - objActivePosition, orts[index]) > Vector2.Dot(objPassivePosition - objActivePosition, -orts[index]))
					return -orts[index] * length;
				else
					return orts[index] * length;
			}

			return new Vector2(0, 0);
		}

		/// <summary>
		/// Return fit-vector for collision circles
		/// </summary>
		/// <param name="objActivePosition"></param>
		/// <param name="objActiveRadius"></param>
		/// <param name="objPassivePosition"></param>
		/// <param name="objPassiveRadius"></param>
		/// <returns></returns>
		public static Vector2 FitObjects(Vector2 objActivePosition, float objActiveRadius, Vector2 objPassivePosition, float objPassiveRadius)
		{
			float length;
			Vector2 ort;
			ort = objActivePosition - objPassivePosition;
			length = ort.Length() - objActiveRadius - objPassiveRadius;
			if (length < 0f)
			{
				ort.Normalize();
				if (Vector2.Dot(objPassivePosition - objActivePosition, ort) > Vector2.Dot(objPassivePosition - objActivePosition, -ort))
					return ort * length;
				else
					return -ort * length;
			}
			return new Vector2(0, 0);
		}

		/// <summary>
		/// Return fit-vector for collision rectangle and circle
		/// </summary>
		/// <param name="objActivePosition"></param>
		/// <param name="objActiveWidth"></param>
		/// <param name="objActiveHeight"></param>
		/// <param name="objActiveRotation"></param>
		/// <param name="objPassivePosition"></param>
		/// <param name="objPassiveRadius"></param>
		/// <returns></returns>
		public static Vector2 FitObjects(Vector2 objActivePosition, float objActiveWidth, float objActiveHeight, float objActiveRotation, Vector2 objPassivePosition, float objPassiveRadius)
		{
			Vector2[] objActivePoints = new Vector2[4];
			Vector2 ort;
			Matrix matRotation = new Matrix();
			float length = 0;

			objActivePoints[0].X = -objActiveWidth / 2f;
			objActivePoints[0].Y = objActiveHeight / 2f;
			objActivePoints[1].X = objActiveWidth / 2f;
			objActivePoints[1].Y = objActiveHeight / 2f;
			objActivePoints[2].X = objActiveWidth / 2f;
			objActivePoints[2].Y = -objActiveHeight / 2f;
			objActivePoints[3].X = -objActiveWidth / 2f;
			objActivePoints[3].Y = -objActiveHeight / 2f;
			matRotation = new Matrix();
			matRotation = Matrix.CreateRotationZ(objActiveRotation);
			for (int i = 0; i < 4; i++)
				objActivePoints[i] = Vector2.Transform(objActivePoints[i], matRotation) + objActivePosition;

			ort = DistanceTo(objPassivePosition, objActivePoints[0], objActivePoints[1]);
			for (int i = 1; i < 4; i++)
			{
				Vector2 tempDistance = DistanceTo(objPassivePosition, objActivePoints[i], objActivePoints[(i + 1) % 4]);
				if (ort.Length() > tempDistance.Length())
					ort = tempDistance;
			}
			length = ort.Length();
			if (ort.Length() < objPassiveRadius)
			{
				ort.Normalize();
				length = objPassiveRadius - length;
				if (Vector2.Dot(objPassivePosition - objActivePosition, ort) > Vector2.Dot(objPassivePosition - objActivePosition, -ort))
					return -ort * length;
				else
					return ort * length;
			}
			return new Vector2(0, 0);
		}

		/// <summary>
		/// Return fit-vector for collision corcle and rectangle
		/// </summary>
		/// <param name="objActivePosition"></param>
		/// <param name="objActiveRadius"></param>
		/// <param name="objPassivePosition"></param>
		/// <param name="objPassiveWidth"></param>
		/// <param name="objPassiveHeight"></param>
		/// <param name="objPassiveRotation"></param>
		/// <returns></returns>
		public static Vector2 FitObjects(Vector2 objActivePosition, float objActiveRadius, Vector2 objPassivePosition, float objPassiveWidth, float objPassiveHeight, float objPassiveRotation)
		{
			return -FitObjects(objPassivePosition, objPassiveWidth, objPassiveHeight, objPassiveRotation, objActivePosition, objActiveRadius);
		}

		/// <summary>
		/// Return fit-vector for collision objects
		/// </summary>
		/// <param name="objActivePos"></param>
		/// <param name="objActiveDir"></param>
		/// <param name="objActiveBound"></param>
		/// <param name="objPassivePos"></param>
		/// <param name="objPassiveDir"></param>
		/// <param name="objPassiveBound"></param>
		/// <returns></returns>
		public static Vector2 FitObjects(Vector2 objActivePos, Vector2 objActiveDir, Bounding objActiveBound, Vector2 objPassivePos, Vector2 objPassiveDir, Bounding objPassiveBound)
		{
			if (objActiveBound.IsRectangle)
			{
				if (objPassiveBound.IsRectangle)
				{
					return FitObjects(objActivePos, ((BoundingRectangle)objActiveBound)._width, ((BoundingRectangle)objActiveBound)._height, (float)Math.Atan2(objActiveDir.Y, objActiveDir.Y), objPassivePos, ((BoundingRectangle)objPassiveBound)._width, ((BoundingRectangle)objPassiveBound)._height, (float)Math.Atan2(objPassiveDir.Y, objPassiveDir.X));
				}
				else
				{
					return FitObjects(objActivePos, ((BoundingRectangle)objActiveBound)._width, ((BoundingRectangle)objActiveBound)._height, (float)Math.Atan2(objActiveDir.Y, objActiveDir.Y), objPassivePos, ((BoundingCircle)objPassiveBound)._radius);
				}
			}
			else
			{
				if (objPassiveBound.IsRectangle)
				{
					return FitObjects(objActivePos, ((BoundingCircle)objActiveBound)._radius, objPassivePos, ((BoundingRectangle)objPassiveBound)._width, ((BoundingRectangle)objPassiveBound)._height, (float)Math.Atan2(objPassiveDir.Y, objPassiveDir.X));
				}
				else
				{
					return FitObjects(objActivePos, ((BoundingCircle)objActiveBound)._radius, objPassivePos, ((BoundingCircle)objPassiveBound)._radius);
				}
			}
			return Vector2.Zero;
		}
	}


}
