using TonPlay.Client.Common.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces
{
	public interface IShopPackItemContext : IScreenContext
	{
		Sprite Icon { get; }
		
		Material BackgroundGradientMaterial { get; }
		
		string AmountText { get; }
	}
}