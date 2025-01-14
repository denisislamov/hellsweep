using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Core.Match;
using TonPlay.Client.Roguelike.Core.Skills.Config;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Profile;
using TonPlay.Client.Roguelike.Shop;
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

		[SerializeField]
		private ShopPackPresentationProvider _shopRewardPresentationProvider;

		public override void InstallBindings()
		{
			SignalBusInstaller.Install(Container);

			Container.Inject(_uiServiceInstaller);
			_uiServiceInstaller.InstallBindings();

			Container.BindInterfacesTo<SceneService.SceneService>().AsSingle();

			UserLoadingServiceInstaller.Install(Container);
			
			Container.BindInterfacesTo<ConfigsLoadingService>().AsSingle();

			var skillConfigProviderInstance = Instantiate(_skillConfigProvider);
			Container.Bind<ISkillConfigProvider>().FromInstance(skillConfigProviderInstance).AsSingle();
			Container.Bind<ISkillConfigUpdater>().To<SkillConfigUpdater>().AsSingle();
			Container.Bind<IShopEmbeddedScreenStorage>().To<ShopEmbeddedScreenStorage>().AsSingle();

			Container.Bind<IShopRewardPresentationProvider>().FromInstance(_shopRewardPresentationProvider).AsSingle();

			MatchInstaller.Install(Container);

			Container.BindFactory<IShopPurchaseActionContext, ShopPurchaseAction, ShopPurchaseAction.Factory>().AsSingle();
		}
	}
}