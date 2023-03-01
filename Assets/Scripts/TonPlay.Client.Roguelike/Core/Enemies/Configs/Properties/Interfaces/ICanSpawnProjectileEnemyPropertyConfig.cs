using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces
{
	public interface ICanSpawnProjectileEnemyPropertyConfig : IEnemyPropertyConfig
	{
		IProjectileConfig ProjectileConfig { get; }

		int Layer { get; }

		int PooledCount { get; }
	}
}