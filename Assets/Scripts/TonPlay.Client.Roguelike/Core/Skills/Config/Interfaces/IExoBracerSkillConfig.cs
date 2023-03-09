namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IExoBracerSkillConfig : ISkillConfig<IExoBracerSkillLevelConfig>
	{
	}

	public interface IExoBracerSkillLevelConfig : ISkillLevelConfig
	{
		float MultiplierValue { get; }
	}
}