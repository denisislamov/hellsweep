using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Effects.Revolver;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config
{
	[CreateAssetMenu(fileName = nameof(CrossbowSkillConfig), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(CrossbowSkillConfig))]
	public class CrossbowSkillConfig : SkillConfig, ICrossbowSkillConfig
	{
		[Header("Crossbow")]
		[SerializeField]
		private ProjectileConfig _projectileConfig;
		
		[SerializeField]
		private CrossbowSightEffect _sightEffectView;
		
		[SerializeField]
		private LevelConfig[] _levelConfigs;
		
		private Dictionary<int, LevelConfig> _map;

		private IReadOnlyDictionary<int, LevelConfig> Map => _map ??= _levelConfigs.ToDictionary(_ => _.Level, _ => _);

		public CrossbowSightEffect SightEffectView => _sightEffectView;
		public IProjectileConfig ProjectileConfig => _projectileConfig;

		public override SkillName SkillName => SkillName.Crossbow;
		
		public ICrossbowLevelSkillConfig GetLevelConfig(int level) => 
			!Map.ContainsKey(level) 
				? null 
				: Map[level];

		[Serializable]
		private class LevelConfig : ICrossbowLevelSkillConfig
		{
			[SerializeField]
			private int _level;
			
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
		}
	}
}