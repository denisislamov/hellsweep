using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public class CollectionItemView : View, ICollectionItemView
	{
		public virtual void SetParent(Transform parent)
		{
			var selfTransform = transform;
			
			selfTransform.SetParent(parent);
			selfTransform.localPosition = Vector3.zero;
			selfTransform.localScale = Vector3.one;
		}
	}
}