using TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;

namespace TonPlay.Roguelike.Client.Core.Pooling.Identities
{
	public class EnemyConfigViewPoolIdentity : IViewPoolIdentity
	{
		public string Id { get; }

		public EnemyConfigViewPoolIdentity(IEnemyConfig config)
		{
			Id = string.Format("Enemy.{0}", config.Id);
		}
	}
}