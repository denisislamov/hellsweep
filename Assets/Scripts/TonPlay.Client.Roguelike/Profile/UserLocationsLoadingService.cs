using System;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.Inventory.Configs.Interfaces;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.Profile
{
	public class UserLocationsLoadingService : IUserLoadingService
	{
		private readonly IMetaGameModelProvider _metaGameModelProvider;
		private readonly IProfileConfigProvider _profileConfigProvider;
		private readonly IRestApiClient _restApiClient;
		
		public UserLocationsLoadingService(
			IMetaGameModelProvider metaGameModelProvider,
			IProfileConfigProvider profileConfigProvider,
			IRestApiClient restApiClient)
		{
			_metaGameModelProvider = metaGameModelProvider;
			_profileConfigProvider = profileConfigProvider;
			_restApiClient = restApiClient;
		}
		
		public async UniTask Load()
		{
		}
		
		public class Factory : PlaceholderFactory<UserLocationsLoadingService>
		{
		}
	}
}