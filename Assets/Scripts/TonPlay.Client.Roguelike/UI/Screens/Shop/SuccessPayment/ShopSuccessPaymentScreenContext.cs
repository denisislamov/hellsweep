using System.Collections.Generic;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SuccessPayment.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SuccessPayment
{
	public class ShopSuccessPaymentScreenContext : ScreenContext, IShopSuccessPaymentScreenContext
	{
		public IReadOnlyList<IShopRewardItemContext> Items { get; }
		
		public ShopSuccessPaymentScreenContext(IReadOnlyList<IShopRewardItemContext> contexts)
		{
			Items = contexts;
		}
	}
}