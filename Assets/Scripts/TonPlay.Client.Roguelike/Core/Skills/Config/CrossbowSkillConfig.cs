using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Effects.Revolver;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config
{
	[CreateAssetMenu(fileName = nameof(CrossbowSkillConfig), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(CrossbowSkillConfig))]
	public class CrossbowSkillConfig : SkillConfig<ICrossbowLevelSkillConfig>, ICrossbowSkillConfig
	{
		[Header("Crossbow")]
		[SerializeField]
		private ProjectileConfig _projectileConfig;

		[SerializeField]
		private CrossbowSightEffect _sightEffectView;

		[SerializeField]
		private LevelConfig[] _levelConfigs;

		private Dictionary<int, LevelConfig> _map;

		private IReadOnlyDictionary<int, LevelConfig> Map => _map ??= _levelConfigs.ToDictionary(_ => _.Level, _ => _.Clone());

		public CrossbowSightEffect SightEffectView => _sightEffectView;
		public IProjectileConfig ProjectileConfig => _projectileConfig;

		public override SkillName SkillName => SkillName.Crossbow;

		public override ICrossbowLevelSkillConfig GetLevelConfig(int level) => GetLevelConfigInternal(level);
		
		public LevelConfig GetLevelConfigInternal(int level) =>
			!Map.ContainsKey(level)
				? null
				: Map[level];
		
		public override void AcceptUpdaterVisitor(ISkillConfigUpdaterVisitor skillConfigUpdaterVisitor) => 
			skillConfigUpdaterVisitor.Update(this);

		[Serializable]
		public class LevelConfig : ICrossbowLevelSkillConfig
		{
			[SerializeField]
			private int _level;

			[SerializeField]
			private string _description;

			[SerializeField]
			private int _projectileQuantity;

			[SerializeField]
			private DamageProvider _damageProvider;

			[SerializeField]
			private float _shootDelay;

			[SerializeField]
			private float _fieldOfView;

			public int Level => _level;
			public int ProjectileQuantity => _projectileQuantity;
			public IDamageProvider DamageProvider => _damageProvider;
			public float ShootDelay => _shootDelay;
			public float FieldOfView => _fieldOfView;
			public string Description => _description;
			
			public void SetDamage(float value)
			{
				_damageProvider.damage = value;
			}
			
			public void SetQuantity(int value)
			{
				_projectileQuantity = value;
			}
			
			public void SetCooldown(float value)
			{
				_shootDelay = value;
			}
			
			public LevelConfig Clone()
			{
				return new LevelConfig()
				{
					_projectileQuantity = _projectileQuantity,
					_shootDelay = _shootDelay,
					_damageProvider = _damageProvider.Clone(),
					_description = (string) _description.Clone(),
					_level = _level,
					_fieldOfView = _fieldOfView
				};
			}
		}
	}
}