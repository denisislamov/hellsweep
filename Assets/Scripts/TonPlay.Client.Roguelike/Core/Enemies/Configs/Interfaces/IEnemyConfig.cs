using TonPlay.Client.Roguelike.Core.Drops.Interfaces;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces;
using TonPlay.Client.Roguelike.Core.Enemies.Views;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces
{
	public interface IEnemyConfig
	{
		public string Id { get; }

		public EnemyType EnemyType { get; }

		public EnemyView Prefab { get; }

		IItemDrop<string>[] RandomCollectableDrops { get; }

		IViewPoolIdentity Identity { get; }
		
		IDeathEffectConfig DeathEffectConfig { get; }

		bool HasProperty<T>() where T : IEnemyPropertyConfig;

		T GetProperty<T>() where T : IEnemyPropertyConfig;
	}
}