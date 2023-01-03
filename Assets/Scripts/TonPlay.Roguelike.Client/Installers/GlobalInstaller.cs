using TonPlay.Roguelike.Client.Bootstrap;
using Zenject;

namespace TonPlay.Roguelike.Client.Installers
{
	public class GlobalInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesTo<BootstrapService>().AsSingle();
		}
	}
}