using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources
{
	public class ShopResourcePopupScreenContext : ScreenContext, IShopResourcePopupScreenContext
	{
		public IShopResourceModel Model { get; }
		public string Title { get; }
		public Sprite Icon { get; }

		public ShopResourcePopupScreenContext(IShopResourceModel model, string title, Sprite icon)
		{
			Model = model;
			Title = title;
			Icon = icon;
		}
	}
}