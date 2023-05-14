using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.UIService.Layers;
using TonPlay.Client.Roguelike.Shop;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks
{
	[CreateAssetMenu(fileName = nameof(ShopPackPopupScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(ShopPackPopupScreenInstaller))]
	public class ShopPackPopupScreenInstaller : ScreenInstaller
	{
		[SerializeField]
		private ShopPackPresentationProvider _presentationProvider; 
			
		[SerializeField]
		private ShopPackItemView _packItemView;
		
		[SerializeField]
		private GameObject _pooledItemsContainerPrefab;
		
		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();

			Bind(subContainer);

			Container.Bind<IScreenFactory<IShopPackPopupScreenContext, ShopPackPopupScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();

			Container.Bind<IScreenFactory<ShopPackPopupScreenContext, ShopPackPopupScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<IShopPackPopupScreenContext, ShopPackPopupScreen>>()
						.To<ShopPackPopupScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IScreenFactory<ShopPackPopupScreenContext, ShopPackPopupScreen>>()
						.To<ShopPackPopupScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);
			
			subContainer.Bind<IShopRewardPresentationProvider>().FromInstance(_presentationProvider).AsSingle();

			subContainer.BindFactory<IShopPackPopupView, IShopPackPopupScreenContext, ShopPackPopupPresenter, ShopPackPopupPresenter.Factory>();
			
			subContainer
			   .BindFactory<IShopPackItemCollectionView, IShopPackItemCollectionContext, ShopPackItemCollectionPresenter, ShopPackItemCollectionPresenter.Factory>()
			   .FromNew();
			
			subContainer.BindFactory<IShopPackItemView, IShopPackItemContext, ShopPackItemPresenter, ShopPackItemPresenter.Factory>();

			var screenHolder = subContainer.Resolve<IUIService>();
			var embeddingRoot = screenHolder.GetScreensRoot(new DefaultScreenLayer());
			var pooledItemsContainer = GameObject.Instantiate(_pooledItemsContainerPrefab, embeddingRoot);

			subContainer
			   .BindMemoryPoolCustomInterface<ShopPackItemView, CollectionItemPool<IShopPackItemView, ShopPackItemView>,
					ICollectionItemPool<IShopPackItemView>>()
			   .FromComponentInNewPrefab(_packItemView)
			   .UnderTransform(pooledItemsContainer.transform);
		}
	}
}