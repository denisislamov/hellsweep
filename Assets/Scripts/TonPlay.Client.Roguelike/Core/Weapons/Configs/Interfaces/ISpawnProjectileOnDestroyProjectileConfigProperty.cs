using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces
{
	public interface ISpawnProjectileOnDestroyProjectileConfigProperty : IProjectileConfigProperty
	{
		IProjectileConfig ProjectileConfig { get; }

		int CollisionLayerMask { get; }
	}
}