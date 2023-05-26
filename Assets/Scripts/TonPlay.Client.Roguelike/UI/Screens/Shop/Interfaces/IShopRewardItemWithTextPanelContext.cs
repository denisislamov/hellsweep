using TonPlay.Client.Common.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces
{
	public interface IShopRewardItemWithTextPanelContext: IShopRewardItemContext
	{
		Color TextPanelColor { get; }
	}
}