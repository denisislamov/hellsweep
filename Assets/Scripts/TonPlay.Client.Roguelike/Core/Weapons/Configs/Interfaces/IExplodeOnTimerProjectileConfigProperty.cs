using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces
{
	public interface IExplodeOnTimerProjectileConfigProperty : IProjectileConfigProperty
	{
		float Timer { get; }

		ICollisionAreaConfig ExplodeCollisionAreConfig { get; }
	}
}