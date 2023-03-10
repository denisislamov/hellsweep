namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IHiPowerMagnetSkillConfig : ISkillConfig<IHiPowerMagnetSkillLevelConfig>
	{
	}
	
	public interface IHiPowerMagnetSkillLevelConfig : ISkillLevelConfig
	{
		float MultiplierValue { get; }
	}
}