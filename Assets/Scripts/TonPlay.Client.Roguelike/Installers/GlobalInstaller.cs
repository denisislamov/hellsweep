using TonPlay.Client.Roguelike.Bootstrap;
using TonPlay.Roguelike.Client.Core.Models;
using Zenject;

namespace TonPlay.Client.Roguelike.Installers
{
	public class GlobalInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesTo<BootstrapService>().AsSingle();
			Container.BindInterfacesTo<GameModelProvider>().AsSingle();
		}
	}
}