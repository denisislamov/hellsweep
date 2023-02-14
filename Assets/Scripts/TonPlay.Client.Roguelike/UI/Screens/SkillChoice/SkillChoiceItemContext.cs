using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.SkillChoice
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
		public Color Color { get; }
		public Sprite LevelIcon { get; }
		public Action<SkillName> ClickedCallback { get; }
		
		public SkillChoiceItemContext(
			SkillName skillName,
			SkillType skillType, 
			string title, 
			string description, 
			Sprite icon, 
			int currentLevel, 
			int maxLevel, 
			Color color,
			Sprite levelIcon,
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
			LevelIcon = levelIcon;
			Color = color;
		}
	}
}