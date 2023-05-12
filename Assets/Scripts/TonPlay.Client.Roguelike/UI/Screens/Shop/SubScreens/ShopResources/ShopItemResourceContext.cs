using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources
{
	public class ShopItemResourceContext : ScreenContext, IShopItemResourceContext
	{
		public string ItemId { get; }
		
		public string ItemDetailId { get; }
		public IShopResourceModel Model { get; }
		public string Title { get; }
		public Material Gradient { get; }
		public Sprite Icon { get; }

		public ShopItemResourceContext(string itemId, string itemDetailId, IShopResourceModel model, string title, Material gradient, Sprite icon)
		{
			ItemId = itemId;
			ItemDetailId = itemDetailId;
			Model = model;
			Title = title;
			Gradient = gradient;
			Icon = icon;
		}
	}
}