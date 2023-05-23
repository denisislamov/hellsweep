using TonPlay.Client.Roguelike.Core.Enemies.Views;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Pooling.Identities
{
	public class EnemyViewPoolIdentity : IViewPoolIdentity
	{
		public string Id { get; }

		public EnemyViewPoolIdentity(EnemyView view)
		{
			Id = string.Format("EnemyView.{0}", view.name);
		}
	}
}