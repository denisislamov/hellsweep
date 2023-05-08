using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces
{
	public interface IShopResourceContext : IScreenContext
	{
		IShopResourceModel Model { get; }
		string Title { get; }
		Material Gradient { get; }
		Sprite Icon { get; }
	}
}