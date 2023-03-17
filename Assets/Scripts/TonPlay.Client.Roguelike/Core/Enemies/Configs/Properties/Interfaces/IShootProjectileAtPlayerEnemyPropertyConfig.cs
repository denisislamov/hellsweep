using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces
{
	public interface IShootProjectileAtPlayerEnemyPropertyConfig : IEnemyPropertyConfig
	{
		IProjectileConfig ProjectileConfig { get; }

		float ShootRateInSeconds { get; }
		
		int Quantity { get; }
		
		float FieldOfView { get; }

		int Layer { get; }

		float MinDistanceToTargetToShoot { get; }

		float MaxDistanceToTargetToShoot { get; }
		
		int PooledProjectileCount { get; }
	}
}