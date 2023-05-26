using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Collectables;

namespace TonPlay.Roguelike.Client.Core.Pooling.Identities
{
	public class CollectableViewPoolIdentity : IViewPoolIdentity
	{
		public string Id { get; }

		public CollectableViewPoolIdentity(CollectableView view)
		{
			Id = string.Format("CollectableView.{0}", view.name);
		}
	}
}