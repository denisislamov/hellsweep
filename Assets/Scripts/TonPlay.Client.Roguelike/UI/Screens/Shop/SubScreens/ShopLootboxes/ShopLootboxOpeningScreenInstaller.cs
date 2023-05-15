using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.UIService.Layers;
using TonPlay.Client.Roguelike.Shop;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopLootboxes
{
	[CreateAssetMenu(fileName = nameof(ShopLootboxOpeningScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(ShopLootboxOpeningScreenInstaller))]
	public class ShopLootboxOpeningScreenInstaller : ScreenInstaller
	{
		[SerializeField]
		private ShopPackPresentationProvider _presentationProvider; 
			
		[SerializeField]
		private ShopRewardItemView _rewardItemView;
		
		[SerializeField]
		private GameObject _pooledItemsContainerPrefab;
		
		
		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();

			Bind(subContainer);

			Container.Bind<IScreenFactory<IShopLootboxOpeningScreenContext, ShopLootboxOpeningScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();

			Container.Bind<IScreenFactory<ShopLootboxOpeningScreenContext, ShopLootboxOpeningScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<IShopLootboxOpeningScreenContext, ShopLootboxOpeningScreen>>()
						.To<ShopLootboxOpeningScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IScreenFactory<ShopLootboxOpeningScreenContext, ShopLootboxOpeningScreen>>()
						.To<ShopLootboxOpeningScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.BindFactory<IShopLootboxOpeningView, IShopLootboxOpeningScreenContext, ShopLootboxOpeningPresenter, ShopLootboxOpeningPresenter.Factory>();
			subContainer.BindFactory<IShopLootboxItemCollectionView, IShopLootboxItemCollectionContext, ShopLootboxItemCollectionPresenter, ShopLootboxItemCollectionPresenter.Factory>();
			subContainer.BindFactory<IShopRewardItemView, IShopRewardItemContext, ShopRewardItemPresenter, ShopRewardItemPresenter.Factory>();
			
			var screenHolder = subContainer.Resolve<IUIService>();
			var embeddingRoot = screenHolder.GetScreensRoot(new DefaultScreenLayer());
			var pooledItemsContainer = GameObject.Instantiate(_pooledItemsContainerPrefab, embeddingRoot);

			subContainer
			   .BindMemoryPoolCustomInterface<ShopRewardItemView, CollectionItemPool<IShopRewardItemView, ShopRewardItemView>,
					ICollectionItemPool<IShopRewardItemView>>()
			   .FromComponentInNewPrefab(_rewardItemView)
			   .UnderTransform(pooledItemsContainer.transform);
		}
	}
}