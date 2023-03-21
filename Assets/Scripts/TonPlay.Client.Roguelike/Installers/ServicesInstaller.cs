using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Core.Match;
using TonPlay.Client.Roguelike.Core.Skills.Config;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
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
		
		[SerializeField]
		private SkillConfigProvider _skillConfigProvider;

		public override void InstallBindings()
		{
			SignalBusInstaller.Install(Container);

			Container.Inject(_uiServiceInstaller);
			_uiServiceInstaller.InstallBindings();

			Container.BindInterfacesTo<SceneService.SceneService>().AsSingle();

			Container.BindInterfacesTo<ProfileLoadingService>().AsSingle();
			Container.BindInterfacesTo<ConfigsLoadingService>().AsSingle();

			var skillConfigProviderInstance = Instantiate(_skillConfigProvider);
			Container.Bind<ISkillConfigProvider>().FromInstance(skillConfigProviderInstance).AsSingle();
			Container.Bind<ISkillConfigUpdater>().To<SkillConfigUpdater>().AsSingle();

			MatchInstaller.Install(Container);
		}
	}
}