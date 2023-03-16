using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Client.Roguelike.Core.Match.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.Core.Match
{
	public class MatchInstaller : Installer<MatchInstaller>
	{
		public override void InstallBindings()
		{
			Container.Bind<IMatchLauncher>().To<MatchLauncher>().AsSingle();
			Container.BindInterfacesTo<MatchProvider>().AsSingle();

			//Container.BindFactory<ILocationConfig, OfflineSingleMatch, OfflineSingleMatch.Factory>().FromNew();
			
			Container.BindFactory<ILocationConfig, SingleMatch, SingleMatch.Factory>().FromNew();
		}
	}
}