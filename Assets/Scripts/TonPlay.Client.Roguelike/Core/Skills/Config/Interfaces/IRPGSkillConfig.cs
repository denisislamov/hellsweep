using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;

namespace TonPlay.Roguelike.Client.Core.Skills.Config.Interfaces
{
	public interface IRPGSkillConfig : ISkillConfig
	{
		IProjectileConfig ProjectileConfig { get; }
		
		IRPGSkillLevelConfig GetLevelConfig(int level);
	}
	
	public interface IRPGSkillLevelConfig 
	{
		float Delay { get; }
	}
}