using System;
using System.Collections.Generic;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Interfaces;
using TonPlay.Roguelike.Client.Core.Skills;
using TonPlay.Roguelike.Client.UI.UIService;

namespace TonPlay.Client.Roguelike.UI.Screens.SkillChoice
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