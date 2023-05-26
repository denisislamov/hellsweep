using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.UIService.Layers;
using TonPlay.Client.Roguelike.Shop;
using TonPlay.Client.Roguelike.UI.Screens.MyBag.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.MyBag
{
	[CreateAssetMenu(fileName = nameof(MyBagPopupScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(MyBagPopupScreenInstaller))]
	public class MyBagPopupScreenInstaller : ScreenInstaller
	{
		[SerializeField]
		private ShopPackPresentationProvider _presentationProvider; 
			
		[SerializeField]
		private ShopRewardItemWithTextPanelView _rewardItemView;
		
		[SerializeField]
		private GameObject _pooledItemsContainerPrefab;
		
		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();

			Bind(subContainer);

			Container.Bind<IScreenFactory<IMyBagPopupScreenContext, MyBagPopupScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();

			Container.Bind<IScreenFactory<MyBagPopupScreenContext, MyBagPopupScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<IMyBagPopupScreenContext, MyBagPopupScreen>>()
						.To<MyBagPopupScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IScreenFactory<MyBagPopupScreenContext, MyBagPopupScreen>>()
						.To<MyBagPopupScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IShopRewardPresentationProvider>().FromInstance(_presentationProvider).AsSingle();
			
			subContainer.BindFactory<IMyBagPopupView, IScreenContext, MyBagPopupPresenter, MyBagPopupPresenter.Factory>();
			
			subContainer
			   .BindFactory<IShopRewardItemCollectionView, IShopRewardItemCollectionContext, ShopRewardItemCollectionPresenter, ShopRewardItemCollectionPresenter.Factory>()
			   .FromNew();
			
			subContainer.BindFactory<IShopRewardItemView, IShopRewardItemContext, ShopRewardItemPresenter, ShopRewardItemPresenter.Factory>();

			subContainer.BindFactory<IMyBagNftPanelView, IMyBagNftPanelContext, MyBagNftPanelPresenter, MyBagNftPanelPresenter.Factory>();

			subContainer.BindFactory<IMyBagItemsPanelView, IScreenContext, MyBagItemsPanelPresenter, MyBagItemsPanelPresenter.Factory>();

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