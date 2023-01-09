using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Views;

namespace TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces
{
	public interface IProjectileConfig
	{
		ProjectileView PrefabView { get; }
		
		float StartSpeed { get; }
		
		float Acceleration { get; }
		
		ICollisionAreaConfig CollisionAreaConfig { get; }
		
		int Damage { get; }
	}
}