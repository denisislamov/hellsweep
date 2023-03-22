using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.AppEntryPoint.Interfaces;
using TonPlay.Client.Roguelike.Bootstrap.Interfaces;
using TonPlay.Client.Roguelike.Interfaces;
using TonPlay.Client.Roguelike.Network.Interfaces;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.Bootstrap
{
	public class BootstrapService : IBootstrapService, IInitializable
	{
		private readonly IAppEntryPoint _appEntryPoint;
		private readonly IRestApiClient _restApiClient;
		private readonly IUserLoadingService _userLoadingService;
		private readonly IConfigsLoadingService _configsLoadingService;

		public BootstrapService(
			IAppEntryPoint appEntryPoint,
			IRestApiClient restApiClient,
			IUserLoadingService userLoadingService,
			IConfigsLoadingService configsLoadingService)
		{
			_appEntryPoint = appEntryPoint;
			_restApiClient = restApiClient;
			_userLoadingService = userLoadingService;
			_configsLoadingService = configsLoadingService;
		}

		public async UniTask Bootstrap()
		{
#if UNITY_WEBGL
			// Default AsyncConversions is Scheduler.ThreadPool
			Scheduler.DefaultSchedulers.AsyncConversions = Scheduler.MainThread;
#endif
			Application.targetFrameRate = -1;

			_restApiClient.Init();

			await _configsLoadingService.Load();

			await _userLoadingService.Load();

			await _appEntryPoint.ProcessEntrance();
		}

		public void Initialize()
		{
			Bootstrap().Forget();
		}
	}
}