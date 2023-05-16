using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Bootstrap;
using TonPlay.Client.Roguelike.Core.Locations;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.Core.Models;
using TonPlay.Client.Roguelike.Core.Player.Configs;
using TonPlay.Client.Roguelike.Core.Player.Configs.Interfaces;
using TonPlay.Client.Roguelike.Inventory.Configs;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Profile;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using TonPlay.Client.Roguelike.Signals;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.Installers
{
	public class GlobalInstaller : MonoInstaller
	{
		[SerializeField]
		private ProfileConfigProvider _profileConfigProvider;

		[SerializeField]
		private LocationConfigProvider _locationConfigProvider;

		[SerializeField]
		private InventoryItemPresentationProvider _inventoryItemPresentationProvider;
		
		[SerializeField]
		private InventoryItemsConfigProvider _inventoryItemsConfigProvider;
		
		[SerializeField]
		private PlayerConfigProvider _playerConfigProvider;

		public override void InstallBindings()
		{
			SignalsInstaller.Install(Container);

			Container.BindInterfacesTo<BootstrapService>().AsSingle();
			Container.BindInterfacesTo<GameModelProvider>().AsSingle();
			Container.BindInterfacesTo<MetaGameModelProvider>().AsSingle();

			Container.BindInterfacesTo<TelegramPlatformProvider>().AsSingle();

			var locationConfigProvider = Instantiate(_locationConfigProvider);
			Container.Bind<ILocationConfigProvider>().FromInstance(locationConfigProvider).AsSingle();
			Container.Bind<ILocationConfigUpdater>().To<LocationConfigUpdater>().AsSingle();
			
			Container.Bind<IPlayerConfigProvider>().FromInstance(_playerConfigProvider).AsSingle();

			var runtimeProfileConfigProvider = Instantiate(_profileConfigProvider);
			
			Container.Bind<IProfileConfigProvider>().FromInstance(runtimeProfileConfigProvider).AsSingle();
			Container.Bind<IProfileConfigProviderUpdater>().FromInstance(runtimeProfileConfigProvider).AsSingle();

			var itemsConfigUpdater = new InventoryItemsConfigUpdater(_inventoryItemsConfigProvider);
			Container.Bind<IInventoryItemsConfigProvider>().FromInstance(_inventoryItemsConfigProvider).AsSingle();
			Container.Bind<IInventoryItemsConfigUpdater>().FromInstance(itemsConfigUpdater).AsSingle();

			var inventoryItemPresentationProvider = Instantiate(_inventoryItemPresentationProvider);
			Container.Bind<IInventoryItemPresentationProvider>().FromInstance(inventoryItemPresentationProvider).AsSingle();

			Container.BindInterfacesTo<LocationConfigStorage>().AsSingle();
		}
	}
}