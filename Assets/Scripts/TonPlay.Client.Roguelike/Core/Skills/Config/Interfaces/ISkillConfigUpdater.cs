using TonPlay.Client.Roguelike.Network.Response;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface ISkillConfigUpdater
	{
		public void UpdateConfig(SkillName skillName, SkillAllResponse.Skill data);
		
		public void UpdateConfig(SkillName skillName, BoostAllResponse.Boost data);
	}
}