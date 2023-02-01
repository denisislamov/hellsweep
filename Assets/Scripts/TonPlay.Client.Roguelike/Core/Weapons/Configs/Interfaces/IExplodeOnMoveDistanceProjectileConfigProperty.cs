using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces
{
	public interface IExplodeOnMoveDistanceProjectileConfigProperty : IProjectileConfigProperty
	{
		float Distance { get; }
		
		int Damage { get; }

		ICollisionAreaConfig ExplodeCollisionAreaConfig { get; }
	}
}