using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Components.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Skills;
using TonPlay.Roguelike.Client.Core.Skills.Config;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config
{
	[CreateAssetMenu(fileName = nameof(BrickSkillConfig), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(BrickSkillConfig))]
	public class BrickSkillConfig : SkillConfig, IBrickSkillConfig
	{
		[Header("Brick")]
		[SerializeField]
		private ProjectileConfig _projectileConfig;
		
		[SerializeField]
		private LevelConfig[] _levelConfigs;
		
		[SerializeField]
		private float _timeToReachDistance;
		
		[SerializeField]
		private float _distanceToThrow;
		
		[SerializeField]
		private float _delayBetweenSpawn;

		private Dictionary<int, LevelConfig> _map;

		private IReadOnlyDictionary<int, LevelConfig> Map => _map ??= _levelConfigs.ToDictionary(_ => _.Level, _ => _);

		public IProjectileConfig ProjectileConfig => _projectileConfig;
		public float TimeToReachDistance => _timeToReachDistance;
		public float DistanceToThrow => _distanceToThrow;
		public float DelayBetweenSpawn => _delayBetweenSpawn;

		public override SkillName SkillName => SkillName.Brick;
		
		public IBrickSkillLevelConfig GetLevelConfig(int level) => 
			!Map.ContainsKey(level) 
				? null 
				: Map[level];

		[Serializable]
		private class LevelConfig : IBrickSkillLevelConfig
		{
			[SerializeField]
			private int _level;
			
			[SerializeField]
			private int _quantity;
			
			[SerializeField]
			private float _cooldown;
			
			[SerializeField]
			private DamageProvider _damageProvider;
			
			[SerializeField]
			private CollisionAreaConfig _collisionAreaConfig;

			public int Level => _level;

			public int Quantity => _quantity;
			public float Cooldown => _cooldown;
			public IDamageProvider DamageProvider => _damageProvider;
			public ICollisionAreaConfig CollisionAreaConfig => _collisionAreaConfig;
		}
	}
}