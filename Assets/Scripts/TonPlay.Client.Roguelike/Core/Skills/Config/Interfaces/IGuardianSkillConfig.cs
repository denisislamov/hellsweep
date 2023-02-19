using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IGuardianSkillConfig : ISkillConfig<IGuardianSkillLevelConfig>
	{
		IProjectileConfig ProjectileConfig { get; }
	}
	
	public interface IGuardianSkillLevelConfig : ISkillLevelConfig
	{
		int Quantity { get; }
		
		float Speed { get; }
		
		float ActiveTime { get; }
		
		float Cooldown { get; }
		
		IDamageProvider DamageProvider { get; }
		
		float Radius { get; }
		
		ICollisionAreaConfig CollisionAreaConfig { get; }
	}
}