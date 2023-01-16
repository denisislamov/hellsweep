using System;
using System.Collections.Generic;
using TonPlay.Roguelike.Client.Core.Skills;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;

namespace TonPlay.Roguelike.Client.UI.Screens.SkillChoice
{
	public class SkillChoiceCollectionContext : ScreenContext, ISkillChoiceCollectionContext
	{
		public IEnumerable<SkillName> Skills { get; }
		
		public Action<SkillName> SkillClickedCallback { get; }

		public SkillChoiceCollectionContext(
			IEnumerable<SkillName> skills, 
			Action<SkillName> skillClickedCallback)
		{
			Skills = skills;
			SkillClickedCallback = skillClickedCallback;
		}
	}
}