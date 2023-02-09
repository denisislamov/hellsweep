using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Profile;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.Installers
{
	public class ServicesInstaller : MonoInstaller
	{
		[SerializeField]
		private UIServiceInstaller _uiServiceInstaller;
		
		public override void InstallBindings()
		{
			SignalBusInstaller.Install(Container);

			Container.Inject(_uiServiceInstaller);
			_uiServiceInstaller.InstallBindings();
			
			Container.BindInterfacesTo<SceneService.SceneService>().AsSingle();

			Container.BindInterfacesTo<ProfileLoadingService>().AsSingle();
		}
	}
}