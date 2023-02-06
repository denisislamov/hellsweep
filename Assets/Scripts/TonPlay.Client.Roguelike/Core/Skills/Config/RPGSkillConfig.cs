using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Components.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Skills;
using TonPlay.Roguelike.Client.Core.Skills.Config;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config
{
	[CreateAssetMenu(fileName = nameof(RPGSkillConfig), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(RPGSkillConfig))]
	public class RPGSkillConfig : SkillConfig, IRPGSkillConfig
	{
		[Header("RPG")]
		[SerializeField]
		private ProjectileConfig _projectileConfig;
		
		[SerializeField]
		private LevelConfig[] _levelConfigs;

		private Dictionary<int, LevelConfig> _map;

		private IReadOnlyDictionary<int, LevelConfig> Map => _map ??= _levelConfigs.ToDictionary(_ => _.Level, _ => _);

		public IProjectileConfig ProjectileConfig => _projectileConfig;
		public override SkillName SkillName => SkillName.RPG;
		
		public IRPGSkillLevelConfig GetLevelConfig(int level) => 
			!Map.ContainsKey(level) 
				? null 
				: Map[level];

		[Serializable]
		private class LevelConfig : IRPGSkillLevelConfig
		{
			[SerializeField]
			private int _level;
			
			[SerializeField]
			private float _delay;

			public int Level => _level;

			public float Delay => _delay;
		}
	}
}