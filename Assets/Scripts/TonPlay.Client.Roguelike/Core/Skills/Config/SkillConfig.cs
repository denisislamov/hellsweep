using System;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config
{
	public abstract class SkillConfig : ScriptableObject, ISkillConfig
	{
		[Header("Common")]
		[SerializeField]
		private SkillType _skillType;

		[SerializeField]
		private string _title;

		[SerializeField]
		private string _description;

		[SerializeField]
		private Sprite _icon;

		[SerializeField]
		private bool _excludeFromInitialDrop;

		[SerializeField]
		private int _maxLevel;

		[SerializeField]
		private SkillName[] _evolutions;

		public abstract SkillName SkillName { get; }

		public SkillType SkillType => _skillType;
		public string Title => _title;
		public string Description => _description;
		public bool ExcludeFromInitialDrop => _excludeFromInitialDrop;
		public Sprite Icon => _icon;
		public int MaxLevel => _maxLevel;
		public SkillName[] Evolutions => _evolutions;
		public abstract string GetLevelDescription(int level);
	}

	public abstract class SkillConfig<T> :
		SkillConfig where T : ISkillLevelConfig
	{
		public abstract T GetLevelConfig(int level);

		public override string GetLevelDescription(int level)
		{
			var config = GetLevelConfig(level);
			return config.Description;
		}
	}
}