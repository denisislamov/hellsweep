using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks
{
	public class ShopPacksView : View, IShopPacksView
	{
		[SerializeField] 
		private ShopPackCollectionView _packCollectionView;
		
		public IShopPackCollectionView PackCollectionView => _packCollectionView;
	}
}