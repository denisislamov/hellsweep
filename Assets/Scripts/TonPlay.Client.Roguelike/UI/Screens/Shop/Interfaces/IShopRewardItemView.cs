using TonPlay.Client.Common.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces
{
	public interface IShopRewardItemView : ICollectionItemView
	{
		void SetIcon(Sprite sprite);

		void SetAmountText(string text);

		void SetBackgroundGradient(Material material);
	}
}