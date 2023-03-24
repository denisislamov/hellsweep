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

		public BootstrapService(
			IAppEntryPoint appEntryPoint,
			IRestApiClient restApiClient)
		{
			_appEntryPoint = appEntryPoint;
			_restApiClient = restApiClient;
		}

		public async UniTask Bootstrap()
		{
#if UNITY_WEBGL
			// Default AsyncConversions is Scheduler.ThreadPool
			Scheduler.DefaultSchedulers.AsyncConversions = Scheduler.MainThread;
#endif
			Application.targetFrameRate = -1;

			_restApiClient.Init();

			await _appEntryPoint.ProcessEntrance();
		}

		public void Initialize()
		{
			Bootstrap().Forget();
		}
	}
}