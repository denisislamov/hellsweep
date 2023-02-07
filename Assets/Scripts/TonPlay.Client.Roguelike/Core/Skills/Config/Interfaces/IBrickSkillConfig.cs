using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Skills.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IBrickSkillConfig : ISkillConfig
	{
		IProjectileConfig ProjectileConfig { get; }
		
		float TimeToReachDistance { get; }
		
		float DistanceToThrow { get; }
		
		float DelayBetweenSpawn { get; }

		IBrickSkillLevelConfig GetLevelConfig(int level);
	}

	public interface IBrickSkillLevelConfig 
	{
		int Quantity { get; }
		
		float Cooldown { get; }
		
		IDamageProvider DamageProvider { get; }
		
		ICollisionAreaConfig CollisionAreaConfig { get; }
	}
}