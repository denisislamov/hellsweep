namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IFitnessGuideSkillConfig : ISkillConfig<IFitnessGuideSkillLevelConfig>
	{
	}
	
	public interface IFitnessGuideSkillLevelConfig : ISkillLevelConfig
	{
		float MultiplierValue { get; }
	}
}