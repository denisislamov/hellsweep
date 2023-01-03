using UnityEngine;
using Zenject;

namespace TonPlay.Roguelike.Client.AppEntryPoint
{
	public class MainAppEntryPointInstaller : MonoInstaller
	{
		[SerializeField]
		private string _entrySceneName;
        
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<MainAppEntryPoint>()
					 .FromSubContainerResolve()
					 .ByMethod(InstallEntryPoint)
					 .AsSingle()
					 .NonLazy();
		}
		
		private void InstallEntryPoint(DiContainer container)
		{
			container.BindInterfacesAndSelfTo<MainAppEntryPoint>().AsSingle();
			container.BindInstance(_entrySceneName);
		}
	}
}