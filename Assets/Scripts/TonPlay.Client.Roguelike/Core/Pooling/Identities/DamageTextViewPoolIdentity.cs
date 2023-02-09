using TonPlay.Client.Roguelike.Core.UI;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;

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