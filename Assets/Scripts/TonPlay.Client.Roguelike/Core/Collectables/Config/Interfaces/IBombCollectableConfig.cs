using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces
{
	public interface IBombCollectableConfig : ICollectableConfig
	{
		ICollisionAreaConfig ExplodeCollisionAreaConfig { get; }
		
		float TimeToExplode { get; }
		
		IDamageProvider DamageProvider { get; }
		
		int LayerMask { get; }
	}
}