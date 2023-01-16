using System.Collections.Generic;

namespace TonPlay.Roguelike.Client.Core.Skills.Config.Interfaces
{
	public interface ISkillConfigProvider
	{
		IEnumerable<ISkillConfig> All { get; }

		ISkillConfig Get(SkillName skillName);
	}
}