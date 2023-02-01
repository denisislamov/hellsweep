using System;
using TonPlay.Roguelike.Client.Core.Skills;
using TonPlay.Roguelike.Client.Core.Skills.Config.Interfaces;
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
		private int _maxLevel;
		
		[SerializeField]
		private SkillName[] _evolutions;

		public abstract SkillName SkillName { get; }
		
		public abstract Type ComponentType { get; }
		
		public SkillType SkillType => _skillType;
		public string Title => _title;
		public string Description => _description;
		public Sprite Icon => _icon;
		public int MaxLevel => _maxLevel;
		public SkillName[] Evolutions => _evolutions;
	}
}