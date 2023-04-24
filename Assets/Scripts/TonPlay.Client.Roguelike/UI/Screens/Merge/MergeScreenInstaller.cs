using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.UIService.Layers;
using TonPlay.Client.Roguelike.UI.Screens.GameSettings;
using TonPlay.Client.Roguelike.UI.Screens.Inventory;
using TonPlay.Client.Roguelike.UI.Screens.Inventory.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Navigation;
using TonPlay.Client.Roguelike.UI.Screens.Merge.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Utilities;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Merge
{
    [CreateAssetMenu(fileName = nameof(MergeScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(MergeScreenInstaller))]
    public class MergeScreenInstaller : ScreenInstaller
    {
        // TODO - finish it like InventoryScreenInstaller
        [SerializeField]
        private GameObject _pooledItemsContainerPrefab;
		
        [SerializeField]
        private InventoryItemView _prefab;
        
        public override void InstallBindings()
        {
            var subContainer = Container.CreateSubContainer();

            Bind(subContainer);
            
            Container.Bind<IScreenFactory<IMergeScreenContext, MergeScreen>>()
                .FromSubContainerResolve()
                .ByInstance(subContainer)
                .AsCached();
            
            // TODO - finish this from InventoryScreenInstaller
        }
        
        private void Bind(DiContainer subContainer)
        {
            subContainer.Bind<IScreenFactory<IMergeScreenContext, MergeScreen>>()
                .To<MergeScreen.Factory>()
                .AsCached()
                .WithArguments(ScreenPrefab);
            
            subContainer.BindFactory<IMergeView, IMergeScreenContext ,MergePresenter, MergePresenter.Factory>();
            subContainer.BindFactory<IProfileBarView, IProfileBarContext, ProfileBarPresenter, ProfileBarPresenter.Factory>();

            NavigationMenuInstaller.Install(subContainer);
			
            var screenHolder = subContainer.Resolve<IUIService>();
            var embeddingRoot = screenHolder.GetScreensRoot(new DefaultScreenLayer());
            var pooledItemsContainer = GameObject.Instantiate(_pooledItemsContainerPrefab, embeddingRoot);

            subContainer
                .BindMemoryPoolCustomInterface<InventoryItemView, CollectionItemPool<IInventoryItemView, InventoryItemView>,
                    ICollectionItemPool<IInventoryItemView>>()
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
            // TODO - finish this from InventoryScreenInstaller
        }
    }
}