using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Components.Enemies.BossFireCaster
{
	public struct BossFireCasterComponent
	{
		public float FollowSpeed;
		
		public float ShootDelay;
		public int ProjectileQuantity;
		public IProjectileConfig ProjectileConfig;

		public float FollowStateDuration;
		public float ShootStateDuration;
		
		public float InitShootDelay;
	}
}