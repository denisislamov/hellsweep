using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;

namespace TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces
{
	public interface IDamageOnCollisionProjectileConfigProperty : IProjectileConfigProperty
	{
		int Damage { get; }

		ICollisionAreaConfig CollisionAreaConfig { get; }
	}
}