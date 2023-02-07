using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct SpawnProjectileOnDestroyComponent
	{
		public IProjectileConfig ProjectileConfig;
		public int CollisionLayerMask;
	}
}