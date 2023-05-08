using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources
{
	public class ShopResourcesView : View, IShopResourcesView
	{
		[SerializeField] 
		private ShopResourceCollectionView _collectionView;
		
		public IShopResourceCollectionView CollectionView => _collectionView;
	}
}