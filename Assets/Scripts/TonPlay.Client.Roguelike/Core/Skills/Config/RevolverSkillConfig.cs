using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
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
	[CreateAssetMenu(fileName = nameof(RevolverSkillConfig), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(RevolverSkillConfig))]
	public class RevolverSkillConfig : SkillConfig<IRevolverLevelSkillConfig>, IRevolverSkillConfig
	{
		[Header("Revolver")]
		[SerializeField]
		private ProjectileConfig _projectileConfig;

		[SerializeField]
		private RevolverSightEffect _sightEffectView;

		[SerializeField]
		private LevelConfig[] _levelConfigs;

		private Dictionary<int, LevelConfig> _map;

		private IReadOnlyDictionary<int, LevelConfig> Map => _map ??= _levelConfigs.ToDictionary(_ => _.Level, _ => _.Clone());

		public RevolverSightEffect SightEffectView => _sightEffectView;
		public IProjectileConfig ProjectileConfig => _projectileConfig;

		public override SkillName SkillName => SkillName.Revolver;

		public override IRevolverLevelSkillConfig GetLevelConfig(int level) => GetLevelConfigInternal(level);
		
		public LevelConfig GetLevelConfigInternal(int level) =>
			!Map.ContainsKey(level)
				? null
				: Map[level];
		
		public override void AcceptUpdaterVisitor(ISkillConfigUpdaterVisitor skillConfigUpdaterVisitor) => 
			skillConfigUpdaterVisitor.Update(this);

		[Serializable]
		public class LevelConfig : IRevolverLevelSkillConfig
		{
			[SerializeField]
			private int _level;

			[SerializeField]
			private string _description;

			[SerializeField]
			private DamageProvider _damageProvider;

			[SerializeField]
			private float _shootDelay;

			[SerializeField]
			private float _fieldOfView;

			[SerializeField]
			private LayerMask _collisionLayerMask;

			[SerializeField]
			private CollisionAreaConfig _collisionAreaConfig;

			public int Level => _level;
			public IDamageProvider DamageProvider => _damageProvider;
			public float ShootDelay => _shootDelay;
			public float FieldOfView => _fieldOfView;
			public int CollisionLayerMask => _collisionLayerMask.value;
			public ICollisionAreaConfig CollisionAreaConfig => _collisionAreaConfig;
			public string Description => _description;
			
			public LevelConfig Clone()
			{
				return new LevelConfig()
				{
					_shootDelay = _shootDelay,
					_fieldOfView = _fieldOfView,
					_collisionLayerMask = _collisionLayerMask,
					_damageProvider = _damageProvider.Clone(),
					_description = (string) _description.Clone(),
					_level = _level,
					_collisionAreaConfig = (CircleCollisionAreaConfig) _collisionAreaConfig.Clone(),
				};
			}
			
			public void SetDamage(float value)
			{
				_damageProvider.damage = value;
			}
			
			public void SetCooldown(float value)
			{
				_shootDelay = value;
			}
		}
	}
}