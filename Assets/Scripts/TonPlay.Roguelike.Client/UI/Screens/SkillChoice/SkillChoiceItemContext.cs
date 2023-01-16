using System;
using TonPlay.Roguelike.Client.Core.Skills;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Roguelike.Client.UI.Screens.SkillChoice
{
	public class SkillChoiceItemContext : ScreenContext, ISkillChoiceItemContext
	{
		public string Title { get; }
		public string Description { get; }
		public Sprite Icon { get; }
		public int CurrentLevel { get; }
		public int MaxLevel { get; }
		public SkillType SkillType { get; }
		public SkillName SkillName { get; }
		public Action<SkillName> ClickedCallback { get; }
		
		public SkillChoiceItemContext(
			SkillName skillName,
			SkillType skillType, 
			string title, 
			string description, 
			Sprite icon, 
			int currentLevel, 
			int maxLevel, 
			Action<SkillName> clickedCallback)
		{
			SkillName = skillName;
			SkillType = skillType;
			Title = title;
			Description = description;
			Icon = icon;
			CurrentLevel = currentLevel;
			MaxLevel = maxLevel;
			ClickedCallback = clickedCallback;
		}
	}
}