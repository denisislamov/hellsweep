using System;
using System.Collections.Generic;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Interfaces
{
	public interface ISkillChoiceCollectionContext : IScreenContext
	{
		IEnumerable<SkillName> Skills { get; }

		Action<SkillName> SkillClickedCallback { get; }
	}
}