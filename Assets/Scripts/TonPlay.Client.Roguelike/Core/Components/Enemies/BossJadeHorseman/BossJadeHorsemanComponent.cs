using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Components.Enemies.BossJadeHorseman
{
	public struct BossJadeHorsemanComponent
	{
		public float ShootDelay;
		public int ShootCount;
		public int ProjectileQuantity;
		public IProjectileConfig ProjectileConfig;
		
		public float ShootStateDuration;

		public int TankCount;
		public float TankPreparingDuration;
		public float TankRunningDuration;
		public float TankStoppingDuration;
		public float TankSpeed;
		public IDamageProvider ShootStateDamageProvider;
		public IDamageProvider TankStateDamageProvider;
	}
}