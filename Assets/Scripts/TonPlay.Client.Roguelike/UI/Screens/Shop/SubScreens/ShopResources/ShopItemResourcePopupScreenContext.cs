using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources
{
	public class ShopItemResourcePopupScreenContext : ScreenContext, IShopItemResourcePopupScreenContext
	{
		private readonly IShopResourcePopupScreenContext _context;
		
		public string ItemDetailId { get; }
		public IShopResourceModel Model => _context.Model;
		public string Title => _context.Title;
		public Sprite Icon => _context.Icon;
		
		public ShopItemResourcePopupScreenContext(string itemDetailId, IShopResourcePopupScreenContext context)
		{
			_context = context;
			ItemDetailId = itemDetailId;
		}
	}
}