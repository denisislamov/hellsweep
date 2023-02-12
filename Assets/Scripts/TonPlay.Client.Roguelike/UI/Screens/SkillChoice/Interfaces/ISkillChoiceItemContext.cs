using System;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Roguelike.Client.Core.Skills;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Interfaces
{
	public interface ISkillChoiceItemContext : IScreenContext
	{
		string Title { get; }
		
		string Description { get; }
		
		Sprite Icon { get; }
		
		int CurrentLevel { get; }
		
		int MaxLevel { get; }
		
		SkillType SkillType { get; }
		
		SkillName SkillName { get; }
		
		Action<SkillName> ClickedCallback { get; }
	}
}