using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Views;

namespace TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces
{
	public interface IProjectileConfig
	{
		ProjectileView PrefabView { get; }

		IMovementConfig MovementConfig { get; }
		
		bool TryGetProperty<T>(out T property) where T : IProjectileConfigProperty;
	}
}