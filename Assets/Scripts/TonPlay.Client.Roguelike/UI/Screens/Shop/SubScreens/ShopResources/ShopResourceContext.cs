using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources
{
	public class ShopResourceContext : ScreenContext, IShopResourceContext
	{
		public IShopResourceModel Model { get; }
		public string Title { get; }
		public Material Gradient { get; }
		public Sprite Icon { get; }

		public ShopResourceContext(IShopResourceModel model, string title, Material gradient, Sprite icon)
		{
			Model = model;
			Title = title;
			Gradient = gradient;
			Icon = icon;
		}
	}
}