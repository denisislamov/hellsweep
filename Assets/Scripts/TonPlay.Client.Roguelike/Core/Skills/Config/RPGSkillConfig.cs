using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config
{
	[CreateAssetMenu(fileName = nameof(RPGSkillConfig), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(RPGSkillConfig))]
	public class RPGSkillConfig : SkillConfig<IRPGSkillLevelConfig>, IRPGSkillConfig
	{
		[Header("RPG")]
		[SerializeField]
		private ProjectileConfig _projectileConfig;

		[SerializeField]
		private LevelConfig[] _levelConfigs;

		private Dictionary<int, LevelConfig> _map;

		private IReadOnlyDictionary<int, LevelConfig> Map => _map ??= _levelConfigs.ToDictionary(_ => _.Level, _ => _.Clone());

		public IProjectileConfig ProjectileConfig => _projectileConfig;
		public override SkillName SkillName => SkillName.RPG;

		public override IRPGSkillLevelConfig GetLevelConfig(int level) => GetLevelConfigInternal(level);
		
		public LevelConfig GetLevelConfigInternal(int level) =>
			!Map.ContainsKey(level)
				? null
				: Map[level];
		
		public override void AcceptUpdaterVisitor(ISkillConfigUpdaterVisitor skillConfigUpdaterVisitor) => 
			skillConfigUpdaterVisitor.Update(this);

		[Serializable]
		public class LevelConfig : IRPGSkillLevelConfig
		{
			[SerializeField]
			private int _level;

			[SerializeField]
			private string _description;

			[SerializeField]
			private float _delay;

			[SerializeField]
			private int _projectileQuantity;

			[SerializeField]
			private DamageProvider _damageProvider;

			public int Level => _level;

			public float Delay => _delay;
			public int ProjectileQuantity => _projectileQuantity;
			public IDamageProvider DamageProvider => _damageProvider;
			public string Description => _description;
			
			public LevelConfig Clone()
			{
				return new LevelConfig()
				{
					_delay = _delay,
					_projectileQuantity = _projectileQuantity,
					_damageProvider = _damageProvider.CloneInternal(),
					_description = (string) _description.Clone(),
					_level = _level,
				};
			}
			
			public void SetDamage(float value)
			{
				_damageProvider.damage = value;
			}
			
			public void SetCooldown(float value)
			{
				_delay = value;
			}
			
			public void SetQuantity(int value)
			{
				_projectileQuantity = value;
			}
		}
	}
}