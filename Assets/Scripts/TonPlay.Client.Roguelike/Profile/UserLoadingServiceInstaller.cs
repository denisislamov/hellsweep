using TonPlay.Client.Roguelike.Profile.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.Profile
{
	public class UserLoadingServiceInstaller : Installer<UserLoadingServiceInstaller>
	{
		public override void InstallBindings()
		{
			Container.Bind<IUserLoadingService>().To<UserLoadingService>().AsSingle();

			Container.BindFactory<UserShopLoadingService, UserShopLoadingService.Factory>().AsSingle();
			Container.BindFactory<UserLocationsLoadingService, UserLocationsLoadingService.Factory>().AsSingle();
			Container.BindFactory<UserInventoryLoadingService, UserInventoryLoadingService.Factory>().AsSingle();
			Container.BindFactory<UserProfileLoadingService, UserProfileLoadingService.Factory>().AsSingle();
			Container.BindFactory<UserGameSettingsLoadingService, UserGameSettingsLoadingService.Factory>().AsSingle();
		}
	}
}