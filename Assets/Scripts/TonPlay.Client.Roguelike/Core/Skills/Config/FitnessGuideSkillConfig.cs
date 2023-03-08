using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config
{
	[CreateAssetMenu(fileName = nameof(FitnessGuideSkillConfig), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(FitnessGuideSkillConfig))]
	public class FitnessGuideSkillConfig : SkillConfig<IFitnessGuideSkillLevelConfig>, IFitnessGuideSkillConfig
	{
		[Header("Fitness Guide")]

		[SerializeField]
		private LevelConfig[] _levelConfigs;

		private Dictionary<int, LevelConfig> _map;

		private IReadOnlyDictionary<int, LevelConfig> Map => _map ??= _levelConfigs.ToDictionary(_ => _.Level, _ => _);

		public override SkillName SkillName => SkillName.FitnessGuide;

		public override IFitnessGuideSkillLevelConfig GetLevelConfig(int level) =>
			!Map.ContainsKey(level)
				? null
				: Map[level];

		[Serializable]
		private class LevelConfig : IFitnessGuideSkillLevelConfig
		{
			[SerializeField]
			private int _level;

			[SerializeField]
			private string _description;
			
			[SerializeField]
			private float _multiplierValue;

			public int Level => _level;

			public string Description => _description;
			
			public float MultiplierValue => _multiplierValue;
		}
	}
}