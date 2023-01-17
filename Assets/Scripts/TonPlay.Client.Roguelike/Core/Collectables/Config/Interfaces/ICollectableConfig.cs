using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;

namespace TonPlay.Roguelike.Client.Core.Collectables.Config.Interfaces
{
	public interface ICollectableConfig
	{
		string Id { get; }
		
		CollectableType Type { get; }
		
		float Value { get; }
		
		CollectableView Prefab { get; }
		
		ICollisionAreaConfig CollisionAreaConfig { get; }
		
		int Layer { get; }
	}
}