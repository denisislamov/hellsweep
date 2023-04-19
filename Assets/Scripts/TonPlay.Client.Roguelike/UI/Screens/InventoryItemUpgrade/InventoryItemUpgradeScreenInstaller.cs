using TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.InventoryItemUpgrade
{
	[CreateAssetMenu(fileName = nameof(InventoryItemUpgradeScreenInstaller), menuName = AssetMenuConstants.SCREENS_INSTALLERS + nameof(InventoryItemUpgradeScreenInstaller))]
	public class InventoryItemUpgradeScreenInstaller : ScreenInstaller
	{
		public override void InstallBindings()
		{
			var subContainer = Container.CreateSubContainer();

			Bind(subContainer);

			Container.Bind<IScreenFactory<IInventoryItemUpgradeScreenContext, InventoryItemUpgradeScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();

			Container.Bind<IScreenFactory<InventoryItemUpgradeScreenContext, InventoryItemUpgradeScreen>>()
					 .FromSubContainerResolve()
					 .ByInstance(subContainer)
					 .AsCached();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.Bind<IScreenFactory<IInventoryItemUpgradeScreenContext, InventoryItemUpgradeScreen>>()
						.To<InventoryItemUpgradeScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.Bind<IScreenFactory<InventoryItemUpgradeScreenContext, InventoryItemUpgradeScreen>>()
						.To<InventoryItemUpgradeScreen.Factory>()
						.AsCached()
						.WithArguments(ScreenPrefab);

			subContainer.BindFactory<
				IInventoryItemUpgradeView, 
				IInventoryItemUpgradeScreenContext, 
				InventoryItemUpgradePresenter, 
				InventoryItemUpgradePresenter.Factory>();
		}
	}
}