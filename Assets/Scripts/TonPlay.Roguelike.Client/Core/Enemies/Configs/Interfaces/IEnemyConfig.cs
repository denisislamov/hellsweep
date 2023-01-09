using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Enemies.Views;

namespace TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces
{
	public interface IEnemyConfig
	{
		public string Id { get; }
		
		public EnemyView Prefab { get; }
		
		int StartHealth { get; }
		
		int DamageOnCollision { get; }
		
		ICollisionAreaConfig CollisionAreaConfig { get; }
	}
}