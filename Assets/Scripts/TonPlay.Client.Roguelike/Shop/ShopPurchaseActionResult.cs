using System.Collections.Generic;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;

namespace TonPlay.Client.Roguelike.Shop
{
	public class ShopPurchaseActionResult : IShopPurchaseActionResult
	{
		public PaymentStatus Status { get; }
		public IReadOnlyList<IShopRewardItemContext> Rewards { get; }
		
		public ShopPurchaseActionResult(PaymentStatus status, IReadOnlyList<IShopRewardItemContext> rewards)
		{
			Status = status;
			Rewards = rewards;
		}
	}
}