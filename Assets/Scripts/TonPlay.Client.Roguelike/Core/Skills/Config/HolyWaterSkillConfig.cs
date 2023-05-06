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
	[CreateAssetMenu(fileName = nameof(HolyWaterSkillConfig), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(HolyWaterSkillConfig))]
	public class HolyWaterSkillConfig : SkillConfig<IHolyWaterSkillLevelConfig>, IHolyWaterSkillConfig
	{
		[Header("Holy Water")]
		[SerializeField]
		private ProjectileConfig _bottleProjectileConfig;

		[SerializeField]
		private ProjectileConfig _damagingAreaProjectileConfig;

		[SerializeField]
		private LayerMask _collisionLayerMask;

		[SerializeField]
		private float _delayBetweenThrowingProjectiles;

		[SerializeField]
		private LevelConfig[] _levelConfigs;

		private Dictionary<int, LevelConfig> _map;

		private IReadOnlyDictionary<int, LevelConfig> Map => _map ??= _levelConfigs.ToDictionary(_ => _.Level, _ => _.Clone());

		public override SkillName SkillName => SkillName.Molotov;

		public IProjectileConfig BottleProjectileConfig => _bottleProjectileConfig;
		public IProjectileConfig DamagingAreaProjectileConfig => _damagingAreaProjectileConfig;

		public float DelayBetweenThrowingProjectiles => _delayBetweenThrowingProjectiles;

		public int CollisionLayerMask => _collisionLayerMask.value;

		public override IHolyWaterSkillLevelConfig GetLevelConfig(int level) => GetLevelConfigInternal(level);
		
		public LevelConfig GetLevelConfigInternal(int level) =>
			!Map.ContainsKey(level)
				? null
				: Map[level];
		
		public override void AcceptUpdaterVisitor(ISkillConfigUpdaterVisitor skillConfigUpdaterVisitor) => 
			skillConfigUpdaterVisitor.Update(this);

		[Serializable]
		public class LevelConfig : IHolyWaterSkillLevelConfig
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
			private float _activeTime;

			[SerializeField]
			private DamageProvider _damageProvider;
			
			[SerializeField]
			private float _radius;

			public int Level => _level;

			public int Quantity => _quantity;

			public float Cooldown => _cooldown;
			public float ActiveTime => _activeTime;

			public IDamageProvider DamageProvider => _damageProvider;
			public string Description => _description;

			public float Radius => _radius;
			
			public LevelConfig Clone()
			{
				return new LevelConfig()
				{
					_quantity = _quantity,
					_activeTime = _activeTime,
					_cooldown = _cooldown,
					_radius = _radius,
					_damageProvider = _damageProvider.CloneInternal(),
					_description = (string) _description.Clone(),
					_level = _level,
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
			
			public void SetCooldown(float value)
			{
				_cooldown = value;
			}
		}
	}
}