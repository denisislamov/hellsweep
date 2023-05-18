using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.UIService.Layers;
using TonPlay.Client.Roguelike.UI.Rewards;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Inventory
{
	[CreateAssetMenu(fileName = nameof(InventoryScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(InventoryScreenInstaller))]
	public class InventoryScreenInstaller : ScreenInstaller
	{
		[SerializeField]
		private GameObject _pooledItemsContainerPrefab;
		
		[SerializeField]
		private InventoryItemView _prefab;

		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();

			Bind(subContainer);

			Container.Bind<IScreenFactory<IInventoryScreenContext, Inventory.InventoryScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();

			Container.Bind<IScreenFactory<InventoryScreenContext, Inventory.InventoryScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<IInventoryScreenContext, Inventory.InventoryScreen>>()
						.To<Inventory.InventoryScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IScreenFactory<InventoryScreenContext, Inventory.InventoryScreen>>()
						.To<Inventory.InventoryScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.BindFactory<IInventoryView, IInventoryScreenContext, InventoryPresenter, InventoryPresenter.Factory>();
			subContainer.BindFactory<IProfileBarView, IProfileBarContext, ProfileBarPresenter, ProfileBarPresenter.Factory>();

			NavigationMenuInstaller.Install(subContainer);
			
			var screenHolder = subContainer.Resolve<IUIService>();
			var embeddingRoot = screenHolder.GetScreensRoot(new DefaultScreenLayer());
			var pooledItemsContainer = GameObject.Instantiate(_pooledItemsContainerPrefab, embeddingRoot);

			subContainer
			   .BindMemoryPoolCustomInterface<InventoryItemView, CollectionItemPool<IInventoryItemView, InventoryItemView>,
					ICollectionItemPool<IInventoryItemView>>()
			   .WithInitialSize(20)
			   .FromComponentInNewPrefab(_prefab)
			   .UnderTransform(pooledItemsContainer.transform);

			subContainer
			   .BindFactory<IInventoryItemView, IInventoryItemContext, InventoryItemPresenter, InventoryItemPresenter.Factory>()
			   .FromNew();
			
			subContainer
			   .BindFactory<IInventoryItemView, IInventoryItemContext, InventoryItemBasePresenter, InventoryItemBasePresenter.Factory>()
			   .FromNew();

			subContainer
			   .BindFactory<IInventoryItemCollectionView, IInventoryItemCollectionContext, InventoryItemCollectionPresenter, InventoryItemCollectionPresenter.Factory>()
			   .FromNew();
			
			subContainer
			   .BindFactory<IInventorySlotView, IInventorySlotContext, InventorySlotPresenter, InventorySlotPresenter.Factory>()
			   .FromNew();
		}
	}
}