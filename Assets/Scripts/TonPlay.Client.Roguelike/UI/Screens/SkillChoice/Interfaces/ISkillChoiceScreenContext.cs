using System;
using System.Collections.Generic;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Interfaces
{
	public interface ISkillChoiceScreenContext : IScreenContext
	{
		IEnumerable<SkillName> SkillsToUpgrade { get; }

		Action<SkillName> SkillChosenCallback { get; }
	}
}