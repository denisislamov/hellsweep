using UnityEngine;

namespace TonPlay.Client.Common.Utilities
{
	public static class Vector2Extensions
	{
		public static Vector2 PerpendicularClockwise(this Vector2 vector2)
		{
			return new Vector2(vector2.y, -vector2.x);
		}

		public static Vector2 PerpendicularCounterClockwise(this Vector2 vector2)
		{
			return new Vector2(-vector2.y, vector2.x);
		}
		
		public static Vector2 Rotate(this Vector2 v, float degrees)
		{
			return v.RotateRadians(degrees * Mathf.Deg2Rad);
		}

		public static Vector2 RotateRadians(this Vector2 v, float radians)
		{
			var ca = Mathf.Cos(radians);
			var sa = Mathf.Sin(radians);
			return new Vector2(ca*v.x - sa*v.y, sa*v.x + ca*v.y);
		}
	}
}