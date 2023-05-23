using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Client.Roguelike.Core.UI;

namespace TonPlay.Client.Roguelike.Core.Pooling.Identities
{
	public class DamageTextViewPoolIdentity : IViewPoolIdentity
	{
		public string Id { get; }

		public DamageTextViewPoolIdentity(DamageTextView damageTextView)
		{
			Id = damageTextView.name;
		}
	}
}