using System;
using TonPlay.Roguelike.Client.Core.Skills;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;

namespace TonPlay.Roguelike.Client.UI.Screens.SkillChoice
{
	public class SkillChoiceScreenContext : ScreenContext, ISkillChoiceScreenContext
	{
		public Action<SkillName> SkillChosenCallback { get; }

		public SkillChoiceScreenContext(Action<SkillName> callback)
		{
			SkillChosenCallback = callback;
		}
	}
}