using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Views;

namespace TonPlay.Roguelike.Client.Core.Player.Configs.Interfaces
{
	public interface IPlayerConfig
	{
		string Id { get; }
		
		PlayerView Prefab { get; }
		
		ICollisionAreaConfig CollisionAreaConfig { get; }
		
		int StartHealth { get; }
		
		IMovementConfig MovementConfig { get; }
	}
}