using System;
using System.Collections.Generic;
using TonPlay.Roguelike.Client.Core.Skills;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Interfaces
{
	public interface ISkillChoiceCollectionContext : IScreenContext
	{
		IEnumerable<SkillName> Skills { get; }
		
		Action<SkillName> SkillClickedCallback { get; }
	}
}