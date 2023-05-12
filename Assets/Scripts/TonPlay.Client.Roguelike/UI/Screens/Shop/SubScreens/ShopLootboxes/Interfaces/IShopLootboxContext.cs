using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces
{
	public interface IShopLootboxContext : IScreenContext
	{
		RarityName Rarity { get; }
	}
}