using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.UIService.Layers;
using TonPlay.Client.Roguelike.Shop;
using TonPlay.Client.Roguelike.UI.Screens.Shop.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Shop.SuccessPayment.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Shop.SuccessPayment
{
	[CreateAssetMenu(fileName = nameof(ShopSuccessPaymentScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(ShopSuccessPaymentScreenInstaller))]
	public class ShopSuccessPaymentScreenInstaller : ScreenInstaller
	{
		[SerializeField]
		private ShopPackPresentationProvider _presentationProvider; 
			
		[SerializeField]
		private ShopRewardItemView _packItemView;
		
		[SerializeField]
		private GameObject _pooledItemsContainerPrefab;
		
		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();

			Bind(subContainer);

			Container.Bind<IScreenFactory<IShopSuccessPaymentScreenContext, ShopSuccessPaymentScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();

			Container.Bind<IScreenFactory<ShopSuccessPaymentScreenContext, ShopSuccessPaymentScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<IShopSuccessPaymentScreenContext, ShopSuccessPaymentScreen>>()
						.To<ShopSuccessPaymentScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IScreenFactory<ShopSuccessPaymentScreenContext, ShopSuccessPaymentScreen>>()
						.To<ShopSuccessPaymentScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);
			
			subContainer.BindFactory<IShopSuccessPaymentView, IShopSuccessPaymentScreenContext, ShopSuccessPaymentPresenter, ShopSuccessPaymentPresenter.Factory>();
			
			subContainer
			   .BindFactory<IShopRewardItemCollectionView, IShopRewardItemCollectionContext, ShopRewardItemCollectionPresenter, ShopRewardItemCollectionPresenter.Factory>()
			   .FromNew();
			
			subContainer.BindFactory<IShopRewardItemView, IShopRewardItemContext, ShopRewardItemPresenter, ShopRewardItemPresenter.Factory>();

			var screenHolder = subContainer.Resolve<IUIService>();
			var embeddingRoot = screenHolder.GetScreensRoot(new DefaultScreenLayer());
			var pooledItemsContainer = GameObject.Instantiate(_pooledItemsContainerPrefab, embeddingRoot);

			subContainer
			   .BindMemoryPoolCustomInterface<ShopRewardItemView, CollectionItemPool<IShopRewardItemView, ShopRewardItemView>,
					ICollectionItemPool<IShopRewardItemView>>()
			   .FromComponentInNewPrefab(_packItemView)
			   .UnderTransform(pooledItemsContainer.transform);
		}
	}
}