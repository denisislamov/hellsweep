namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface ISportShoesSkillConfig : ISkillConfig<ISportShoesSkillLevelConfig>
	{
	}
	
	public interface ISportShoesSkillLevelConfig : ISkillLevelConfig
	{
		float MultiplierValue { get; }
	}
}