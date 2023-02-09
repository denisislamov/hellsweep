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
	[CreateAssetMenu(fileName = nameof(GuardianSkillConfig), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(GuardianSkillConfig))]
	public class GuardianSkillConfig : SkillConfig, IGuardianSkillConfig
	{
		[Header("Guardian")]
		[SerializeField]
		private ProjectileConfig _projectileConfig;
		
		[SerializeField]
		private LevelConfig[] _levelConfigs;

		private Dictionary<int, LevelConfig> _map;

		private IReadOnlyDictionary<int, LevelConfig> Map => _map ??= _levelConfigs.ToDictionary(_ => _.Level, _ => _);

		public IProjectileConfig ProjectileConfig => _projectileConfig;
		
		public override SkillName SkillName => SkillName.Guardian;
		
		public IGuardianSkillLevelConfig GetLevelConfig(int level) => 
			!Map.ContainsKey(level) 
				? null 
				: Map[level];

		[Serializable]
		private class LevelConfig : IGuardianSkillLevelConfig
		{
			[SerializeField]
			private int _level;
			
			[SerializeField]
			private int _quantity;
			
			[SerializeField]
			private float _speed;
			
			[SerializeField]
			private float _activeTime;
			
			[SerializeField]
			private float _cooldown;
			
			[SerializeField]
			private DamageProvider _damageProvider;
			
			[SerializeField]
			private float _radius;
			
			[SerializeField]
			private CollisionAreaConfig _collisionAreaConfig;

			public int Level => _level;

			public int Quantity => _quantity;
			public float Speed => _speed;
			public float ActiveTime => _activeTime;
			public float Cooldown => _cooldown;
			public IDamageProvider DamageProvider => _damageProvider;
			public float Radius => _radius;
			
			public ICollisionAreaConfig CollisionAreaConfig => _collisionAreaConfig;
		}
	}
}