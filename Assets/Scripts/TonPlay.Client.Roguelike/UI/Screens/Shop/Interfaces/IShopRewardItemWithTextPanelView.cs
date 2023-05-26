using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces
{
	public interface IShopRewardItemWithTextPanelView : IShopRewardItemView
	{
		void SetTextPanelColor(Color color);
	}
}