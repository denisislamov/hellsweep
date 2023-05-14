using System.Collections.Generic;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SuccessPayment.Interfaces
{
	public interface IShopSuccessPaymentScreenContext : IScreenContext
	{
		IReadOnlyList<IShopRewardItemContext> Items { get; }
	}
}