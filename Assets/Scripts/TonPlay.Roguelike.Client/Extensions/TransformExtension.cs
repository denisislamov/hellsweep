using UnityEngine;

namespace TonPlay.Roguelike.Client.Extensions
{
	public static class TransformExtension
	{
		public static void Reset(this Transform transform)
		{
			transform.localPosition = Vector3.zero;
			transform.rotation = Quaternion.identity;
			transform.localScale = Vector3.one;
		}
	}
}