namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IRoninOyoroiSkillConfig : ISkillConfig<IRoninOyoroiSkillLevelConfig>
	{
	}
	
	public interface IRoninOyoroiSkillLevelConfig : ISkillLevelConfig
	{
		float MultiplierValue { get; }
	}
}