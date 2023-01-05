using TonPlay.Roguelike.Client.Core.Enemies.Views;

namespace TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces
{
	public interface IEnemySpawnConfig
	{
		public string Id { get; }
		
		public EnemyView Prefab { get; }
		
		int StartHealth { get; }
		
		int DamageOnCollision { get; }
	}
}