using System.Collections.Generic;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Common.UIService.Layers;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Layers;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Common.UIService
{
	[CreateAssetMenu(fileName = nameof(UIServiceInstaller), menuName = AssetMenuConstants.SERVICE_INSTALLERS + nameof(UIServiceInstaller))]
	public class UIServiceInstaller : ScriptableObjectInstaller<UIServiceInstaller>
	{
		[SerializeField]
		private DefaultScreenRoot _defaultScreenRoot;

		[SerializeField]
		private ScreenInstaller[] _screenInstallers;

		[SerializeField]
		private ButtonsInstaller _buttonsInstaller;

		public override void InstallBindings()
		{
			Container.Bind<IUIService>()
					 .FromSubContainerResolve()
					 .ByMethod(Bind)
					 .AsSingle();
		}

		private void Bind(DiContainer subContainer)
		{
			subContainer.BindInterfacesTo<DefaultScreenRoot>().FromComponentInNewPrefab(_defaultScreenRoot).AsSingle();
			
			subContainer.Bind<IEnumerable<IScreenLayer>>()
						.FromInstance(new IScreenLayer[]
						 {
							 new DefaultScreenLayer(),
							 new TutorialScreenLayer(),
							 new LoadingScreenLayer()
						 })
						.AsSingle();

			subContainer.BindInterfacesTo<ScreenFactoryFacade>().AsSingle();
			
			subContainer.BindInterfacesTo<UIService>().AsSingle();

			foreach (var screenInstaller in _screenInstallers)
			{
				subContainer.Inject(screenInstaller);
				screenInstaller.InstallBindings();
			}
			
			subContainer.Inject(_buttonsInstaller);
			_buttonsInstaller.InstallBindings();
		}
	}
}