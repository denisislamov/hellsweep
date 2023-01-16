using System;
using TonPlay.Roguelike.Client.Core.Skills;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Interfaces
{
	public interface ISkillChoiceScreenContext : IScreenContext
	{
		Action<SkillName> SkillChosenCallback { get; }
	}
}