using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Player.Views;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using TonPlay.Roguelike.Client.Core.Player.Views;

namespace TonPlay.Client.Roguelike.Core.Player.Configs.Interfaces
{
	public interface IPlayerConfig
	{
		string Id { get; }

		PlayerView Prefab { get; }

		ICollisionAreaConfig CollisionAreaConfig { get; }

		int StartHealth { get; }

		IMovementConfig MovementConfig { get; }

		int CollisionAreaMask { get; }
	}
}