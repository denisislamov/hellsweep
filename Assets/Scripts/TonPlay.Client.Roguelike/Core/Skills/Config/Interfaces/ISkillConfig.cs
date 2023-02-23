using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface ISkillConfig
	{
		SkillName SkillName { get; }

		SkillType SkillType { get; }

		string Title { get; }

		bool ExcludeFromInitialDrop { get; }

		Sprite Icon { get; }

		int MaxLevel { get; }

		SkillName[] Evolutions { get; }

		string GetLevelDescription(int level);
	}

	public interface ISkillConfig<out T> : ISkillConfig
		where T : ISkillLevelConfig
	{
		T GetLevelConfig(int level);
	}
}