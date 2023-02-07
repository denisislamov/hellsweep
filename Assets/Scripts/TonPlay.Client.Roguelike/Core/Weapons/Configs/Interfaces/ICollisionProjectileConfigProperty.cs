using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces
{
	public interface ICollisionProjectileConfigProperty : IProjectileConfigProperty
	{
		ICollisionAreaConfig CollisionAreaConfig { get; }
	}
}