using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config
{
	[CreateAssetMenu(fileName = nameof(DrillShotSkillConfig), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(DrillShotSkillConfig))]
	public class DrillShotSkillConfig : SkillConfig<IDrillShotSkillLevelConfig>, IDrillShotSkillConfig
	{
		[Header("DrillShot")]
		[SerializeField]
		private ProjectileConfig _projectileConfig;

		[SerializeField]
		private LevelConfig[] _levelConfigs;

		[SerializeField]
		private Rect _flyingZone;

		[SerializeField]
		private float _delayBetweenSpawn;

		private Dictionary<int, LevelConfig> _map;

		private IReadOnlyDictionary<int, LevelConfig> Map => _map ??= _levelConfigs.ToDictionary(_ => _.Level, _ => _);

		public IProjectileConfig ProjectileConfig => _projectileConfig;
		
		public Rect FlyingZone => _flyingZone;
		public float DelayBetweenSpawn => _delayBetweenSpawn;

		public override SkillName SkillName => SkillName.DrillShot;

		public override IDrillShotSkillLevelConfig GetLevelConfig(int level) =>
			!Map.ContainsKey(level)
				? null
				: Map[level];

		[Serializable]
		private class LevelConfig : IDrillShotSkillLevelConfig
		{
			[SerializeField]
			private int _level;

			[SerializeField]
			private string _description;

			[SerializeField]
			private float _speed;
			
			[SerializeField]
			private int _quantity;

			[SerializeField]
			private float _cooldown;
			
			[SerializeField]
			private float _activeTime;

			[SerializeField]
			private DamageProvider _damageProvider;

			[SerializeField]
			private CollisionAreaConfig _collisionAreaConfig;

			public int Level => _level;

			public int Quantity => _quantity;
			
			public float Speed => _speed;
			public float Cooldown => _cooldown;
			public float ActiveTime => _activeTime;
			public IDamageProvider DamageProvider => _damageProvider;
			public ICollisionAreaConfig CollisionAreaConfig => _collisionAreaConfig;
			public string Description => _description;
		}
	}
}