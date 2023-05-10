using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces
{
	public interface IShopPackPopupScreenContext : IScreenContext
	{
		IShopPackModel Model { get; }
	}
}