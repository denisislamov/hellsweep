using TonPlay.Roguelike.Client.Core.Skills;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface ISkillConfig
	{
		SkillName SkillName { get; }
		
		SkillType SkillType { get; }
		
		string Title { get; }
		
		string Description { get; }

		bool ExcludeFromInitialDrop { get;}
		
		Sprite Icon { get; }
		
		int MaxLevel { get; }
		
		SkillName[] Evolutions { get; }
	}
}