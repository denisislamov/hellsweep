using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.UIService.Layers;
using TonPlay.Client.Roguelike.Shop;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SubScreens.ShopPacks
{
	[CreateAssetMenu(fileName = nameof(ShopPacksScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(ShopPacksScreenInstaller))]
	public class ShopPacksScreenInstaller : ScreenInstaller
	{
		[SerializeField]
		private ShopPackPresentationProvider _presentationProvider; 
			
		[SerializeField]
		private ShopPackView _packView;
		
		[SerializeField]
		private ShopPackItemView _packItemView;
		
		[SerializeField]
		private GameObject _pooledItemsContainerPrefab;
		
		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();

			Bind(subContainer);

			Container.Bind<IScreenFactory<IShopPacksScreenContext, ShopPacksScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();

			Container.Bind<IScreenFactory<ShopPacksScreenContext, ShopPacksScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<IShopPacksScreenContext, ShopPacksScreen>>()
						.To<ShopPacksScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IScreenFactory<ShopPacksScreenContext, ShopPacksScreen>>()
						.To<ShopPacksScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IShopRewardPresentationProvider>().FromInstance(_presentationProvider).AsSingle();

			subContainer.BindFactory<IShopPacksView, IShopPacksScreenContext, ShopPacksPresenter, ShopPacksPresenter.Factory>();
			
			subContainer.BindFactory<IShopPackView, IShopPackContext, ShopPackPresenter, ShopPackPresenter.Factory>();
			
			subContainer.BindFactory<IShopPackItemView, IShopPackItemContext, ShopPackItemPresenter, ShopPackItemPresenter.Factory>();
			
			subContainer
			   .BindFactory<IShopPackCollectionView, IShopPackCollectionContext, ShopPackCollectionPresenter, ShopPackCollectionPresenter.Factory>()
			   .FromNew();
			
			subContainer
			   .BindFactory<IShopPackItemCollectionView, IShopPackItemCollectionContext, ShopPackItemCollectionPresenter, ShopPackItemCollectionPresenter.Factory>()
			   .FromNew();

			var screenHolder = subContainer.Resolve<IUIService>();
			var embeddingRoot = screenHolder.GetScreensRoot(new DefaultScreenLayer());
			var pooledItemsContainer = GameObject.Instantiate(_pooledItemsContainerPrefab, embeddingRoot);

			subContainer
			   .BindMemoryPoolCustomInterface<ShopPackView, CollectionItemPool<IShopPackView, ShopPackView>,
					ICollectionItemPool<IShopPackView>>()
			   .FromComponentInNewPrefab(_packView)
			   .UnderTransform(pooledItemsContainer.transform);
			
			subContainer
			   .BindMemoryPoolCustomInterface<ShopPackItemView, CollectionItemPool<IShopPackItemView, ShopPackItemView>,
					ICollectionItemPool<IShopPackItemView>>()
			   .FromComponentInNewPrefab(_packItemView)
			   .UnderTransform(pooledItemsContainer.transform);
		}
	}
}