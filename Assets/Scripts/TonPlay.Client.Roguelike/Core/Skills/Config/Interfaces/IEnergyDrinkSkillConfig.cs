namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface IEnergyDrinkSkillConfig : ISkillConfig<IEnergyDrinkSkillLevelConfig>
	{
	}
	
	public interface IEnergyDrinkSkillLevelConfig : ISkillLevelConfig
	{
		float IncreaseHealthMultiplier { get; }
	}
}