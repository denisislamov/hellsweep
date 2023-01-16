using System;
using System.Collections.Generic;
using TonPlay.Roguelike.Client.Core.Skills;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;

namespace TonPlay.Roguelike.Client.UI.Screens.SkillChoice
{
	public class SkillChoiceScreenContext : ScreenContext, ISkillChoiceScreenContext
	{
		public IEnumerable<SkillName> SkillsToUpgrade { get; }
		public Action<SkillName> SkillChosenCallback { get; }

		public SkillChoiceScreenContext(IEnumerable<SkillName> skillsToUpgrade, Action<SkillName> callback)
		{
			SkillsToUpgrade = skillsToUpgrade;
			SkillChosenCallback = callback;
		}
	}
}