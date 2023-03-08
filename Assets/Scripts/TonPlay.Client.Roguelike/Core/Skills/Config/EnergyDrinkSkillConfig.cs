using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config
{
	[CreateAssetMenu(fileName = nameof(EnergyDrinkSkillConfig), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(EnergyDrinkSkillConfig))]
	public class EnergyDrinkSkillConfig : SkillConfig<IEnergyDrinkSkillLevelConfig>, IEnergyDrinkSkillConfig
	{
		[Header("Energy Drink")]

		[SerializeField]
		private LevelConfig[] _levelConfigs;

		private Dictionary<int, LevelConfig> _map;

		private IReadOnlyDictionary<int, LevelConfig> Map => _map ??= _levelConfigs.ToDictionary(_ => _.Level, _ => _);

		public override SkillName SkillName => SkillName.EnergyDrink;

		public override IEnergyDrinkSkillLevelConfig GetLevelConfig(int level) =>
			!Map.ContainsKey(level)
				? null
				: Map[level];

		[Serializable]
		private class LevelConfig : IEnergyDrinkSkillLevelConfig
		{
			[SerializeField]
			private int _level;

			[SerializeField]
			private string _description;
			
			[SerializeField]
			private float _increaseHealthMultiplier;

			public int Level => _level;

			public string Description => _description;
			
			public float IncreaseHealthMultiplier => _increaseHealthMultiplier;
		}
	}
}