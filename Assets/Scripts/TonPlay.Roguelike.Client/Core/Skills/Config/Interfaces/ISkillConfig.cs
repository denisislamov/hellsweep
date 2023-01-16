using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Skills.Config.Interfaces
{
	public interface ISkillConfig
	{
		SkillName SkillName { get; }
		
		SkillType SkillType { get; }
		
		string Title { get; }
		
		string Description { get; }
		
		Sprite Icon { get; }
		
		int MaxLevel { get; }
		
		SkillName[] Evolutions { get; }
	}
}