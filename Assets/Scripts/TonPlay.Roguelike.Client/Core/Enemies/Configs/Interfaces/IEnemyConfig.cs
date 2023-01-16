using System.Collections.Generic;
using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Enemies.Views;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;

namespace TonPlay.Roguelike.Client.Core.Enemies.Configs.Interfaces
{
	public interface IEnemyConfig
	{
		public string Id { get; }
		
		public EnemyView Prefab { get; }
		
		int StartHealth { get; }
		
		int DamageOnCollision { get; }
		
		IMovementConfig MovementConfig { get; }
		
		ICollisionAreaConfig CollisionAreaConfig { get; }
		
		IReadOnlyList<string> CollectablesIdsOnDeath { get; }
	}
}