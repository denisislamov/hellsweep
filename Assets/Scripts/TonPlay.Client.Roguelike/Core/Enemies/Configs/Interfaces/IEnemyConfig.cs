using TonPlay.Client.Roguelike.Core.Drops.Interfaces;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces;
using TonPlay.Client.Roguelike.Core.Enemies.Views;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces
{
	public interface IEnemyConfig
	{
		public string Id { get; }
		
		public EnemyType EnemyType { get; }
		
		public EnemyView Prefab { get; }
		
		int StartHealth { get; }
		
		IItemDrop<string>[] RandomCollectableDrops { get; }
		
		IViewPoolIdentity Identity { get; }

		bool HasProperty<T>() where T : IEnemyPropertyConfig;
		
		T GetProperty<T>() where T : IEnemyPropertyConfig;
	}
}