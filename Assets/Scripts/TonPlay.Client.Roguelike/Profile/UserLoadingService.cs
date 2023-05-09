using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Profile.Interfaces;

namespace TonPlay.Client.Roguelike.Profile
{
	public class UserLoadingService : IUserLoadingService
	{
		private readonly IUserLoadingService _userShopLoadingService;
		private readonly IUserLoadingService _userProfileLoadingService;
		private readonly IUserLoadingService _userLocationsLoadingService;
		private readonly IUserLoadingService _userInventoryLoadingService;
		private readonly IUserLoadingService _userGameSettingsLoadingService;

		public UserLoadingService(
			UserShopLoadingService.Factory userShopLoadingServiceFactory,
			UserProfileLoadingService.Factory userProfileLoadingServiceFactory,
			UserLocationsLoadingService.Factory userLocationsLoadingServiceFactory,
			UserInventoryLoadingService.Factory userInventoryLoadingServiceFactory,
			UserGameSettingsLoadingService.Factory userGameSettingsLoadingServiceFactory)
		{
			_userShopLoadingService = userShopLoadingServiceFactory.Create();
			_userProfileLoadingService = userProfileLoadingServiceFactory.Create();
			_userLocationsLoadingService = userLocationsLoadingServiceFactory.Create();
			_userInventoryLoadingService = userInventoryLoadingServiceFactory.Create();
			_userGameSettingsLoadingService = userGameSettingsLoadingServiceFactory.Create();
		}

		public async UniTask Load()
		{
			await _userProfileLoadingService.Load();
			await _userLocationsLoadingService.Load();
			await _userInventoryLoadingService.Load();
			await _userGameSettingsLoadingService.Load();
			await _userShopLoadingService.Load();
		}
	}
}