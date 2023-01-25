using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces
{
	public interface IBombCollectableConfig : ICollectableConfig
	{
		ICollisionAreaConfig CollisionAreaConfig { get; }
		
		float TimeToExplode { get; }
		
		float Damage { get; }
		
		int LayerMask { get; }
	}
}