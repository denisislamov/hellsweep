using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Components.Enemies.BossWorm
{
	public struct BossWormComponent
	{
		public float FollowSpeed;
		
		public float ShootDelay;
		public int ProjectileQuantity;
		public IProjectileConfig ProjectileConfig;
		
		public float FollowStateDuration;
		public float ShootStateDuration;
		
		public float TankPreparingDuration;
		public float TankRunningDuration;
		public float TankStoppingDuration;
		public float TankSpeed;
	}
}