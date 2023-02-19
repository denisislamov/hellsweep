using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IBrickSkillConfig : ISkillConfig<IBrickSkillLevelConfig>
	{
		IProjectileConfig ProjectileConfig { get; }
		
		float TimeToReachDistance { get; }
		
		float DistanceToThrow { get; }
		
		float DelayBetweenSpawn { get; }
	}

	public interface IBrickSkillLevelConfig : ISkillLevelConfig
	{
		int Quantity { get; }
		
		float Cooldown { get; }
		
		IDamageProvider DamageProvider { get; }
		
		ICollisionAreaConfig CollisionAreaConfig { get; }
	}
}