using Cysharp.Threading.Tasks;
using TonPlay.Roguelike.Client.AppEntryPoint.Interfaces;
using TonPlay.Roguelike.Client.Bootstrap.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace TonPlay.Roguelike.Client.Bootstrap
{
	public class BootstrapService : IBootstrapService, IInitializable
	{
		private readonly IAppEntryPoint _appEntryPoint;
		
		public BootstrapService(IAppEntryPoint appEntryPoint)
		{
			_appEntryPoint = appEntryPoint;
		}
		
		public async UniTask Bootstrap()
		{
#if UNITY_WEBGL
			// Default AsyncConversions is Scheduler.ThreadPool
			Scheduler.DefaultSchedulers.AsyncConversions = Scheduler.MainThread;
#endif

			Application.targetFrameRate = 30;
			
			await _appEntryPoint.ProcessEntrance();
		}
		
		public void Initialize()
		{
			Bootstrap().Forget();
		}
	}
}