using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface ISkillConfigProvider
	{
		IEnumerable<ISkillConfig> All { get; }
		ISkillPresentationConfig PresentationConfig { get; }

		ISkillConfig Get(SkillName skillName);
	}
}