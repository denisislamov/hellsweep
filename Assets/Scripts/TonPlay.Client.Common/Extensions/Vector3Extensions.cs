using UnityEngine;

namespace TonPlay.Client.Common.Extensions
{
	public static class Vector3Extensions
	{
		public static Vector2 ToVector2XY(this Vector3 vector) => new Vector2(vector.x, vector.y);
		public static Vector2 ToVector2XZ(this Vector3 vector) => new Vector2(vector.x, vector.z);
	}
}