using TonPlay.Client.Roguelike.Core.Drops.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Enemies.Views;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces
{
	public interface IEnemyConfig
	{
		public string Id { get; }
		
		public EnemyView Prefab { get; }
		
		int StartHealth { get; }
		
		IDamageProvider DamageProvider { get; }
		
		IMovementConfig MovementConfig { get; }
		
		ICollisionAreaConfig CollisionAreaConfig { get; }
		
		IItemDrop<string>[] RandomCollectableDrops { get; }
	}
}