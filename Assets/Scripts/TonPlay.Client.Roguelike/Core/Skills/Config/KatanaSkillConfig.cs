using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config
{
	[CreateAssetMenu(fileName = nameof(KatanaSkillConfig), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(KatanaSkillConfig))]
	public class KatanaSkillConfig : SkillConfig<IKatanaLevelSkillConfig>, IKatanaSkillConfig
	{
		[Header("Katana")]
		[SerializeField]
		private ProjectileConfig _projectileConfig;

		[SerializeField]
		private LevelConfig[] _levelConfigs;

		private Dictionary<int, LevelConfig> _map;

		private IReadOnlyDictionary<int, LevelConfig> Map => _map ??= _levelConfigs.ToDictionary(_ => _.Level, _ => _.Clone());

		public IProjectileConfig ProjectileConfig => _projectileConfig;

		public override SkillName SkillName => SkillName.Katana;

		public override IKatanaLevelSkillConfig GetLevelConfig(int level) => GetLevelConfigInternal(level);
		
		public LevelConfig GetLevelConfigInternal(int level) =>
			!Map.ContainsKey(level)
				? null
				: Map[level];
		
		public override void AcceptUpdaterVisitor(ISkillConfigUpdaterVisitor skillConfigUpdaterVisitor) => 
			skillConfigUpdaterVisitor.Update(this);

		[Serializable]
		public class LevelConfig : IKatanaLevelSkillConfig
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
			private float _cooldown;

			[SerializeField]
			private float _shootDelay;
			
			[SerializeField]
			private float _prepareAttackTiming;

			[SerializeField]
			private Vector2 _spawnOffset;

			public int Level => _level;
			public int ProjectileQuantity => _projectileQuantity;
			public IDamageProvider DamageProvider => _damageProvider;
			public float Cooldown => _cooldown;
			public float PrepareAttackTiming => _prepareAttackTiming;
			public float ShootDelay => _shootDelay;
			public Vector2 SpawnOffset => _spawnOffset;
			public string Description => _description;
			
			public LevelConfig Clone()
			{
				return new LevelConfig()
				{
					_projectileQuantity = _projectileQuantity,
					_prepareAttackTiming = _prepareAttackTiming,
					_shootDelay = _shootDelay,
					_cooldown = _cooldown,
					_spawnOffset = _spawnOffset,
					_damageProvider = _damageProvider.CloneInternal(),
					_description = (string) _description.Clone(),
					_level = _level,
				};
			}
			
			public void SetDamage(float value)
			{
				_damageProvider.damage = value;
			}
		}
	}
}