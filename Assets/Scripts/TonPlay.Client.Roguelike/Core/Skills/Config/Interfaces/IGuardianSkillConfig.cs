using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Skills.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IGuardianSkillConfig : ISkillConfig
	{
		IProjectileConfig ProjectileConfig { get; }
		
		IGuardianSkillLevelConfig GetLevelConfig(int level);
	}
	
	public interface IGuardianSkillLevelConfig 
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