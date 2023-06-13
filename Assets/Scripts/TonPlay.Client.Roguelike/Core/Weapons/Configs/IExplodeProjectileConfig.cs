using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs
{
	public interface IExplodeProjectileConfig
	{
		ICollisionAreaConfig ExplodeCollisionAreaConfig { get; }
		
		IDamageProvider DamageProvider { get; }
		
		int ExplodeCollisionLayerMask { get; }
	}
}