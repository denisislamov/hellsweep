using TonPlay.Roguelike.Client.Core.Skills.Config.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Skills.Config
{
	[CreateAssetMenu(fileName = nameof(SkillConfig), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(SkillConfig))]
	public class SkillConfig : ScriptableObject, ISkillConfig
	{
		[SerializeField]
		private SkillName _skillName;
		
		[SerializeField]
		private SkillType _skillType;

		[SerializeField]
		private string _title;
		
		[SerializeField]
		private string _description;
		
		[SerializeField]
		private Sprite _icon;

		[SerializeField]
		private int _maxLevel;
		
		[SerializeField]
		private SkillName[] _evolutions;

		public SkillName SkillName => _skillName;
		public SkillType SkillType => _skillType;
		public string Title => _title;
		public string Description => _description;
		public Sprite Icon => _icon;
		public int MaxLevel => _maxLevel;
		public SkillName[] Evolutions => _evolutions;
	}
}