using TonPlay.Roguelike.Client.Core.Movement.Interfaces;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Views;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces
{
	public interface IProjectileConfig
	{
		ProjectileView PrefabView { get; }

		IMovementConfig MovementConfig { get; }

		IViewPoolIdentity Identity { get; }

		int Layer { get; }

		bool HasProperty<T>() where T : IProjectileConfigProperty;

		T GetProperty<T>() where T : IProjectileConfigProperty;
	}
}