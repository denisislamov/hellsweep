using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Components.Enemies.BossButcher
{
	public struct BossButcherComponent
	{
		public float FollowSpeed;
		public float FollowStateDuration;
		public IDamageProvider FollowDamageProvider;
		
		public float TankPreparingDuration;
		public float TankRunningDuration;
		public float TankStoppingDuration;
		public float TankSpeed;
		public IDamageProvider TankDamageProvider;
	}
}