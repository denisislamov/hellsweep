using UnityEngine;

namespace TonPlay.Client.Common.Extensions
{
	public static class LayerMaskExt
	{
		public static bool ContainsLayer(int layerMask, int layer)
		{
			return (layerMask & (1 << layer)) != 0;
		}
	}
}