using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces
{
	public interface ISkillPresentationConfig
	{
		string DefenceIconText { get; }

		string UtilityIconText { get; }

		Color DefenceColor { get; }

		Color UtilityColor { get; }

		Color UltimateDefenceColor { get; }

		Sprite DefenceLevelIcon { get; }

		Sprite UtilityLevelIcon { get; }

		Sprite UltimateDefenceLevelIcon { get; }
	}
}