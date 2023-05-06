using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config
{
	[CreateAssetMenu(fileName = nameof(GuardianSkillConfig), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(GuardianSkillConfig))]
	public class GuardianSkillConfig : SkillConfig<IGuardianSkillLevelConfig>, IGuardianSkillConfig
	{
		[Header("Guardian")]
		[SerializeField]
		private ProjectileConfig _projectileConfig;

		[SerializeField]
		private LevelConfig[] _levelConfigs;

		private Dictionary<int, LevelConfig> _map;

		private IReadOnlyDictionary<int, LevelConfig> Map => _map ??= _levelConfigs.ToDictionary(_ => _.Level, _ => _.Clone());

		public IProjectileConfig ProjectileConfig => _projectileConfig;

		public override SkillName SkillName => SkillName.Guardian;

		public override IGuardianSkillLevelConfig GetLevelConfig(int level) => GetLevelConfigInternal(level);
		
		public LevelConfig GetLevelConfigInternal(int level) =>
			!Map.ContainsKey(level)
				? null
				: Map[level];
		
		public override void AcceptUpdaterVisitor(ISkillConfigUpdaterVisitor skillConfigUpdaterVisitor) => 
			skillConfigUpdaterVisitor.Update(this);

		[Serializable]
		public class LevelConfig : IGuardianSkillLevelConfig
		{
			[SerializeField]
			private int _level;

			[SerializeField]
			private string _description;

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
			public string Description => _description;
			
			public LevelConfig Clone()
			{
				return new LevelConfig()
				{
					_quantity = _quantity,
					_speed = _speed,
					_activeTime = _activeTime,
					_cooldown = _cooldown,
					_radius = _radius,
					_damageProvider = _damageProvider.CloneInternal(),
					_description = (string) _description.Clone(),
					_level = _level,
					_collisionAreaConfig = (CircleCollisionAreaConfig) _collisionAreaConfig.Clone(),
				};
			}
			
			public void SetDamage(float value)
			{
				_damageProvider.damage = value;
			}
			
			public void SetQuantity(int value)
			{
				_quantity = value;
			}
			
			public void SetRange(float value)
			{
				_radius = value;
			}
			
			public void SetDuration(float value)
			{
				_activeTime = value;
			}
			
			public void SetCooldown(float value)
			{
				_cooldown = value;
			}
			
			public void SetSpinSpeed(float value)
			{
				_speed = value;
			}
		}
	}
}