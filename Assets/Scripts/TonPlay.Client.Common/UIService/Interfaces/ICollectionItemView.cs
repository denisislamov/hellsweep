using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Common.UIService.Interfaces
{
	public interface ICollectionItemView : IView
	{
		void SetParent(Transform parent);
	}
}