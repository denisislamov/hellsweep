using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces
{
	public interface IExplodeOnMoveDistanceProjectileConfigProperty : IProjectileConfigProperty
	{
		float Distance { get; }
		
		IDamageProvider DamageProvider { get; }

		ICollisionAreaConfig ExplodeCollisionAreaConfig { get; }
	}
}