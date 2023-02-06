using System.Collections.Generic;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;

namespace TonPlay.Roguelike.Client.Core.Skills.Config.Interfaces
{
	public interface ISkillConfigProvider
	{
		IEnumerable<ISkillConfig> All { get; }

		ISkillConfig Get(SkillName skillName);
	}
}