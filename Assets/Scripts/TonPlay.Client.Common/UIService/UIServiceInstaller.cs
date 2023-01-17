using System.Collections.Generic;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Layers;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;
using Zenject;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	[CreateAssetMenu(fileName = nameof(UIServiceInstaller), menuName = AssetMenuConstants.SERVICE_INSTALLERS + nameof(UIServiceInstaller))]
	public class UIServiceInstaller : ScriptableObjectInstaller<UIServiceInstaller>
	{
		[SerializeField]
		private DefaultScreenRoot _defaultScreenRoot;

		[SerializeField]
		private ScreenInstaller[] _screenInstallers;

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
							 new TutorialScreenLayer()
						 })
						.AsSingle();

			subContainer.BindInterfacesTo<ScreenFactoryFacade>().AsSingle();
			
			subContainer.BindInterfacesTo<UIService>().AsSingle();

			foreach (var screenInstaller in _screenInstallers)
			{
				subContainer.Inject(screenInstaller);
				screenInstaller.InstallBindings();
			}
		}
	}
}