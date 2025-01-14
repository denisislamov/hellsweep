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
	[CreateAssetMenu(fileName = nameof(BrickSkillConfig), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(BrickSkillConfig))]
	public class BrickSkillConfig : SkillConfig<IBrickSkillLevelConfig>, IBrickSkillConfig
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

		private IReadOnlyDictionary<int, LevelConfig> Map => _map ??= _levelConfigs.ToDictionary(_ => _.Level, _ => _.Clone());

		public IProjectileConfig ProjectileConfig => _projectileConfig;
		public float TimeToReachDistance => _timeToReachDistance;
		public float DistanceToThrow => _distanceToThrow;
		public float DelayBetweenSpawn => _delayBetweenSpawn;

		public override SkillName SkillName => SkillName.Brick;
		
		public override void AcceptUpdaterVisitor(ISkillConfigUpdaterVisitor skillConfigUpdaterVisitor) => 
			skillConfigUpdaterVisitor.Update(this);

		public override IBrickSkillLevelConfig GetLevelConfig(int level) => GetLevelConfigInternal(level);
		
		public LevelConfig GetLevelConfigInternal(int level) =>
			!Map.ContainsKey(level)
				? null
				: Map[level];

		[Serializable]
		public class LevelConfig : IBrickSkillLevelConfig
		{
			[SerializeField]
			private int _level;

			[SerializeField]
			private string _description;

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
			public string Description => _description;

			public void SetDamage(float damage)
			{
				_damageProvider.damage = damage;
			}
			
			public void SetQuantity(int value)
			{
				_quantity = value;
			}
			
			public LevelConfig Clone()
			{
				return new LevelConfig()
				{
					_collisionAreaConfig = _collisionAreaConfig.Clone(),
					_cooldown = _cooldown,
					_damageProvider = _damageProvider.CloneInternal(),
					_description = (string) _description.Clone(),
					_level = _level,
					_quantity = _quantity
				};
			}
		}
	}
}