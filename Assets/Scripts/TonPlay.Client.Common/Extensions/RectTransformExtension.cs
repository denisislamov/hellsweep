using UnityEngine;

namespace TonPlay.Client.Common.Extensions
{
	public static class RectTransformExtension
	{
		public static void ResetRectSize(this RectTransform rectTransform)
		{
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
		}
	}
}