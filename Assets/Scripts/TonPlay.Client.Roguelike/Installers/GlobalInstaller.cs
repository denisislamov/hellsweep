using TonPlay.Client.Roguelike.Bootstrap;
using TonPlay.Client.Roguelike.Core.Models;
using TonPlay.Client.Roguelike.Models;
using TonPlay.Client.Roguelike.Profile;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using TonPlay.Client.Roguelike.Signals;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.Installers
{
	public class GlobalInstaller : MonoInstaller
	{
		[SerializeField]
		private ProfileConfigProvider _profileConfigProvider;
		
		public override void InstallBindings()
		{
			SignalsInstaller.Install(Container);
			
			Container.BindInterfacesTo<BootstrapService>().AsSingle();
			Container.BindInterfacesTo<GameModelProvider>().AsSingle();
			Container.BindInterfacesTo<MetaGameModelProvider>().AsSingle();
			Container.Bind<IProfileConfigProvider>().FromInstance(_profileConfigProvider).AsSingle();
		}
	}
}