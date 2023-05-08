using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks
{
	public class ShopPackContext : ScreenContext, IShopPackContext
	{
		public IShopPackModel ShopPackModel { get; }
		public string Title { get; }
		public Color MainColor { get; }
		public Material Gradient { get; }
		
		public ShopPackContext(IShopPackModel shopPackModel, string title, Color mainColor, Material gradient)
		{
			ShopPackModel = shopPackModel;
			Title = title;
			MainColor = mainColor;
			Gradient = gradient;
		}
	}
}