using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Components.Enemies.ShadowCasterMiniboss
{
	public struct ShadowCasterComponent
	{
		public float FollowSpeed;
		
		public float ShootDelay;
		public int ProjectileQuantity;
		public IProjectileConfig ProjectileConfig;
		public IDamageProvider ShootDamageProvider;

		public float FollowStateDuration;
		public float ShootStateDuration;
		
		public IDamageProvider FollowDamageProvider;
		public float InitShootDelay;
	}
}