using TonPlay.Client.Common.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces
{
	public interface IShopRewardItemContext : IScreenContext
	{
		Sprite Icon { get; }
		
		Material BackgroundGradientMaterial { get; }
		
		string AmountText { get; }
	}
}