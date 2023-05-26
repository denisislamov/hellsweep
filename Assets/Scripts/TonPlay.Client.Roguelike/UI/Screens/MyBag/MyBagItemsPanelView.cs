using TonPlay.Client.Roguelike.UI.Screens.MyBag.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.MyBag
{
	public class MyBagItemsPanelView : View, IMyBagItemsPanelView
	{
		[SerializeField] 
		private ShopRewardItemCollectionView _itemsCollectionView;
		
		public IShopRewardItemCollectionView ItemsCollectionView => _itemsCollectionView;
	}
}