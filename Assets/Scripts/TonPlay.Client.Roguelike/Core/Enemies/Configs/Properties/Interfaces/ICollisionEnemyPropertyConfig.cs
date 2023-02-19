using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces
{
	public interface ICollisionEnemyPropertyConfig : IEnemyPropertyConfig
	{
		ICollisionAreaConfig CollisionAreaConfig { get; }
		
		int LayerMask { get; }
	}
}