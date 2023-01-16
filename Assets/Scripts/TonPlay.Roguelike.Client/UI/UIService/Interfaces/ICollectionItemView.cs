using UnityEngine;

namespace TonPlay.Roguelike.Client.UI.UIService.Interfaces
{
	public interface ICollectionItemView : IView
	{
		void SetParent(Transform parent);
	}
}