using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.UIService.Layers;
using TonPlay.Client.Roguelike.Shop;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopResources
{
	[CreateAssetMenu(fileName = nameof(ShopResourcesScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(ShopResourcesScreenInstaller))]
	public class ShopResourcesScreenInstaller : ScreenInstaller
	{
		[SerializeField]
		private ShopResourcePresentationProvider _presentationProvider; 
			
		[SerializeField]
		private ShopResourceView _resourceView;
		
		[SerializeField]
		private GameObject _pooledItemsContainerPrefab;
		
		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();

			Bind(subContainer);

			Container.Bind<IScreenFactory<IShopResourcesScreenContext, ShopResourcesScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();

			Container.Bind<IScreenFactory<ShopResourcesScreenContext, ShopResourcesScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<IShopResourcesScreenContext, ShopResourcesScreen>>()
						.To<ShopResourcesScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IScreenFactory<ShopResourcesScreenContext, ShopResourcesScreen>>()
						.To<ShopResourcesScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.BindFactory<IShopResourcesView, IShopResourcesScreenContext, ShopResourcesPresenter, ShopResourcesPresenter.Factory>();
			
			subContainer.Bind<IShopResourcePresentationProvider>().FromInstance(_presentationProvider).AsSingle();

			subContainer.BindFactory<IShopResourceView, IShopResourceContext, ShopResourcePresenter, ShopResourcePresenter.Factory>();
			
			subContainer
			   .BindFactory<IShopResourceCollectionView, IShopResourceCollectionContext, ShopResourceCollectionPresenter, ShopResourceCollectionPresenter.Factory>()
			   .FromNew();
			
			var screenHolder = subContainer.Resolve<IUIService>();
			var embeddingRoot = screenHolder.GetScreensRoot(new DefaultScreenLayer());
			var pooledItemsContainer = GameObject.Instantiate(_pooledItemsContainerPrefab, embeddingRoot);

			subContainer
			   .BindMemoryPoolCustomInterface<ShopResourceView, CollectionItemPool<IShopResourceView, ShopResourceView>,
					ICollectionItemPool<IShopResourceView>>()
			   .FromComponentInNewPrefab(_resourceView)
			   .UnderTransform(pooledItemsContainer.transform);
		}
	}
}