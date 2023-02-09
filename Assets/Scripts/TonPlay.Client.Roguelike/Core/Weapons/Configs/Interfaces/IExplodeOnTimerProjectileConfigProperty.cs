using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;

namespace TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces
{
	public interface IExplodeOnTimerProjectileConfigProperty : IProjectileConfigProperty
	{
		float Timer { get; }
		
		ICollisionAreaConfig ExplodeCollisionAreConfig { get; }
	}
}