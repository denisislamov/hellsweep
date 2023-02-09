using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public abstract class CollectionView<TItemView> : View, ICollectionView<TItemView>
		where TItemView : ICollectionItemView
	{
		[SerializeField]
		private RectTransform _contentContainer = null;

		public void AddContent(TItemView item)
		{
			SetItemParent(item);
			RebuildLayout();
		}
		
		public void RebuildLayout()
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(_contentContainer);
			LayoutRebuilder.ForceRebuildLayoutImmediate(_contentContainer);
		}

		protected virtual void SetItemParent(TItemView item)
		{
			item.SetParent(_contentContainer);
		}
	}
}