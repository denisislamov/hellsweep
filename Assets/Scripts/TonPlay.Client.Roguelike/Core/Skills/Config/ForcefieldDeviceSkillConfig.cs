using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Effects;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.CollisionAreas.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Skills;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Skills.Config
{
	[CreateAssetMenu(fileName = nameof(ForcefieldDeviceSkillConfig), menuName = AssetMenuConstants.SKILLS_CONFIGS + nameof(ForcefieldDeviceSkillConfig))]
	public class ForcefieldDeviceSkillConfig : SkillConfig, IForcefieldDeviceSkillConfig
	{
		[Header("Forcefield Device")]

		[SerializeField]
		private EffectView _effectView;

		[SerializeField]
		private LevelConfig[] _levelConfigs;

		private Dictionary<int, LevelConfig> _map;

		private IReadOnlyDictionary<int, LevelConfig> Map => _map ??= _levelConfigs.ToDictionary(_ => _.Level, _ => _);

		public override SkillName SkillName => SkillName.ForcefieldDevice;

		public EffectView EffectView => _effectView;
		
		public IForcefieldDeviceSkillLevelConfig GetLevelConfig(int level) => 
			!Map.ContainsKey(level) 
				? null 
				: Map[level];

		[Serializable]
		private class LevelConfig : IForcefieldDeviceSkillLevelConfig
		{
			[SerializeField]
			private int _level;
			
			[SerializeField]
			private float _damage;
			
			[SerializeField]
			private float _size;

			[SerializeField]
			private LayerMask _collisionLayerMask;

			[SerializeField]
			private CollisionAreaConfig _collisionAreaConfig;

			public int Level => _level;

			public float Damage => _damage;
			
			public float Size => _size;

			public int CollisionLayerMask => _collisionLayerMask.value;

			public ICollisionAreaConfig CollisionAreaConfig => _collisionAreaConfig;
		}
	}
}