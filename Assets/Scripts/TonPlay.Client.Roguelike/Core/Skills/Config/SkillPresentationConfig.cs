using System;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace TonPlay.Client.Roguelike.Core.Skills.Config
{
	[Serializable]
	public class SkillPresentationConfig : ISkillPresentationConfig
	{
		[SerializeField]
		private string _defenceIconText;

		[SerializeField]
		private string _utilityIconText;

		[SerializeField]
		private Color _defenceColor;

		[SerializeField]
		private Color _utilityColor;

		[SerializeField]
		private Color _ultimateDefenceColor;

		[SerializeField]
		private Sprite _defenceLevelIcon;

		[SerializeField]
		private Sprite _utilityLevelIcon;

		[SerializeField]
		private Sprite _ultimateDefenceLevelIcon;

		public string DefenceIconText => _defenceIconText;
		public string UtilityIconText => _utilityIconText;
		public Color DefenceColor => _defenceColor;
		public Color UtilityColor => _utilityColor;
		public Color UltimateDefenceColor => _ultimateDefenceColor;
		public Sprite DefenceLevelIcon => _defenceLevelIcon;
		public Sprite UtilityLevelIcon => _utilityLevelIcon;
		public Sprite UltimateDefenceLevelIcon => _ultimateDefenceLevelIcon;
	}
}