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

		private IReadOnlyDictionary<int, LevelConfig> Map => _map ??= _levelConfigs.ToDictionary(_ => _.Level, _ => _.Clone());

		public IProjectileConfig ProjectileConfig => _projectileConfig;
		
		public Rect FlyingZone => _flyingZone;
		public float DelayBetweenSpawn => _delayBetweenSpawn;

		public override SkillName SkillName => SkillName.DrillShot;

		public override IDrillShotSkillLevelConfig GetLevelConfig(int level) => GetLevelConfigInternal(level);
		
		public LevelConfig GetLevelConfigInternal(int level) =>
			!Map.ContainsKey(level)
				? null
				: Map[level];
		
		public override void AcceptUpdaterVisitor(ISkillConfigUpdaterVisitor skillConfigUpdaterVisitor) => 
			skillConfigUpdaterVisitor.Update(this);

		[Serializable]
		public class LevelConfig : IDrillShotSkillLevelConfig
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
			
			public void SetDamage(float value)
			{
				_damageProvider.damage = value;
			}
			
			public void SetQuantity(int value)
			{
				_quantity = value;
			}
			
			public void SetSpeed(float value)
			{
				_speed = value;
			}
			
			public LevelConfig Clone()
			{
				return new LevelConfig()
				{
					_quantity = _quantity,
					_speed = _speed,
					_cooldown = _cooldown,
					_activeTime = _activeTime,
					_damageProvider = _damageProvider.Clone(),
					_description = (string) _description.Clone(),
					_level = _level,
					_collisionAreaConfig = _collisionAreaConfig.Clone(),
				};
			}
		}
	}
}