namespace TonPlay.Roguelike.Client.Core.Skills.Config.Interfaces
{
	public interface ISkillConfigProvider
	{
		ISkillConfig Get(SkillName skillName);
	}
}