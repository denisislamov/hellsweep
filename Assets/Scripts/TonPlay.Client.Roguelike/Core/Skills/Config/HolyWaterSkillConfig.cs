using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config
{
	[CreateAssetMenu(fileName = nameof(HolyWaterSkillConfig), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(HolyWaterSkillConfig))]
	public class HolyWaterSkillConfig : SkillConfig, IHolyWaterSkillConfig
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

		private IReadOnlyDictionary<int, LevelConfig> Map => _map ??= _levelConfigs.ToDictionary(_ => _.Level, _ => _);

		public override SkillName SkillName => SkillName.Molotov;

		public IProjectileConfig BottleProjectileConfig => _bottleProjectileConfig;
		public IProjectileConfig DamagingAreaProjectileConfig => _damagingAreaProjectileConfig;
		
		public float DelayBetweenThrowingProjectiles => _delayBetweenThrowingProjectiles;
		
		public int CollisionLayerMask => _collisionLayerMask.value;

		public IHolyWaterSkillLevelConfig GetLevelConfig(int level) => 
			!Map.ContainsKey(level) 
				? null 
				: Map[level];

		[Serializable]
		private class LevelConfig : IHolyWaterSkillLevelConfig
		{
			[SerializeField]
			private int _level;
			
			[SerializeField]
			private int _quantity;
			
			[SerializeField]
			private float _cooldown;
			
			[SerializeField]
			private DamageProvider _damageProvider;

			public int Level => _level;
			
			public int Quantity => _quantity;
			
			public float Cooldown => _cooldown;
			
			public IDamageProvider DamageProvider => _damageProvider;
		}
	}
}