using System.Collections.Generic;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;

namespace TonPlay.Client.Roguelike.Shop
{
	public interface IShopPurchaseActionResult
	{
		PaymentStatus Status { get; }
		
		IReadOnlyList<IShopRewardItemContext> Rewards { get; }
	}
}