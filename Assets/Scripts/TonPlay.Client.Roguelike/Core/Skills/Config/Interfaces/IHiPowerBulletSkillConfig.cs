namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IHiPowerBulletSkillConfig : ISkillConfig<IHiPowerBulletSkillLevelConfig>
	{
	}
	
	public interface IHiPowerBulletSkillLevelConfig : ISkillLevelConfig
	{
		float MultiplierValue { get; }
	}
}