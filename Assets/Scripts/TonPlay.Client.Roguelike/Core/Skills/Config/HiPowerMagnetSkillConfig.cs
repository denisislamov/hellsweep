using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config
{
	[CreateAssetMenu(fileName = nameof(HiPowerMagnetSkillConfig), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(HiPowerMagnetSkillConfig))]
	public class HiPowerMagnetSkillConfig : SkillConfig<IHiPowerMagnetSkillLevelConfig>, IHiPowerMagnetSkillConfig
	{
		[Header("Hi Power Magnet")]
		[SerializeField]
		private LevelConfig[] _levelConfigs;

		private Dictionary<int, LevelConfig> _map;

		private IReadOnlyDictionary<int, LevelConfig> Map => _map ??= _levelConfigs.ToDictionary(_ => _.Level, _ => _.Clone());

		public override SkillName SkillName => SkillName.HIPowerMagnet;

		public override IHiPowerMagnetSkillLevelConfig GetLevelConfig(int level) => GetLevelConfigInternal(level);
		
		public LevelConfig GetLevelConfigInternal(int level) =>
			!Map.ContainsKey(level)
				? null
				: Map[level];
		
		public override void AcceptUpdaterVisitor(ISkillConfigUpdaterVisitor skillConfigUpdaterVisitor) => 
			skillConfigUpdaterVisitor.Update(this);

		[Serializable]
		public class LevelConfig : IHiPowerMagnetSkillLevelConfig
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
			
			public void SetValue(float value)
			{
				_multiplierValue = value;
			}
			
			public LevelConfig Clone()
			{
				return new LevelConfig()
				{
					_description = (string) _description.Clone(),
					_level = _level,
					_multiplierValue = _multiplierValue,
				};
			}
		}
	}
}