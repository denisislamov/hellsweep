using Zenject;

namespace TonPlay.Client.Roguelike.Signals
{
	public class SignalsInstaller : Installer<SignalsInstaller>
	{
		public override void InstallBindings()
		{
			Container.DeclareSignal<AppliedDamageSignal>().OptionalSubscriber();
		}
	}
}