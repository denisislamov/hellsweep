using Cysharp.Threading.Tasks;
using TonPlay.Client.Roguelike.AppEntryPoint.Interfaces;
using TonPlay.Client.Roguelike.Bootstrap.Interfaces;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.Bootstrap
{
	public class BootstrapService : IBootstrapService, IInitializable
	{
		private readonly IAppEntryPoint _appEntryPoint;
		private readonly IProfileLoadingService _profileLoadingService;

		public BootstrapService(
			IAppEntryPoint appEntryPoint,
			IProfileLoadingService profileLoadingService)
		{
			_appEntryPoint = appEntryPoint;
			_profileLoadingService = profileLoadingService;
		}

		public async UniTask Bootstrap()
		{
#if UNITY_WEBGL
			// Default AsyncConversions is Scheduler.ThreadPool
			Scheduler.DefaultSchedulers.AsyncConversions = Scheduler.MainThread;
#endif
			Application.targetFrameRate = -1;

			await _profileLoadingService.Load();

			await _appEntryPoint.ProcessEntrance();
		}

		public void Initialize()
		{
			Bootstrap().Forget();
		}
	}
}