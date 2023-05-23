using TonPlay.Client.Roguelike.Core.Effects;
using TonPlay.Client.Roguelike.Core.Enemies.Views;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Pooling.Identities
{
	public class DeathEffectViewPoolIdentity : IViewPoolIdentity
	{
		public string Id { get; }

		public DeathEffectViewPoolIdentity(DeathEffectView view)
		{
			Id = string.Format("DeathEffect.{0}", view.name);
		}
	}
}