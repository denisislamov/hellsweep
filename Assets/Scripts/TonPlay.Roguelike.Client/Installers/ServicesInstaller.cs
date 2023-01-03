using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using Zenject;

namespace TonPlay.Roguelike.Client.Installers
{
	public class ServicesInstaller : MonoInstaller
	{
		[SerializeField]
		private UIServiceInstaller _uiServiceInstaller;
		
		public override void InstallBindings()
		{
			Container.Inject(_uiServiceInstaller);
			_uiServiceInstaller.InstallBindings();
			
			Container.BindInterfacesTo<SceneService.SceneService>().AsSingle();
		}
	}
}