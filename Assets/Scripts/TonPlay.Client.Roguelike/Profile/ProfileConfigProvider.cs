using System;
using System.Collections.Generic;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Profile
{
	[CreateAssetMenu(fileName = nameof(ProfileConfigProvider), menuName = AssetMenuConstants.CONFIGS + nameof(ProfileConfigProvider))]
	public class ProfileConfigProvider : ScriptableObject, IProfileConfigProvider, IProfileConfigProviderUpdater
	{
		[SerializeField]
		private List<ProfileConfig> _configs;

		private Dictionary<int, ProfileConfig> _internalConfigs;

		public IProfileConfig Get(int level) => GetInternal(level);

		private ProfileConfig GetInternal(int level)
		{
			_internalConfigs ??= CreateConfigsCopy();

			return !_internalConfigs.ContainsKey(level) 
				? default 
				: _internalConfigs[level];
		}
		
		private Dictionary<int, ProfileConfig> CreateConfigsCopy()
		{
			var map = new Dictionary<int, ProfileConfig>(_configs.Count);

			for (var i = 0; i < _configs.Count; i++)
			{
				var copy = _configs[i].Copy();
				
				map.Add(copy.level, copy);
			}

			return map;
		}

		[Serializable]
		private class ProfileConfig : IProfileConfig
		{
			public int level;

			[SerializeField]
			internal float _experienceToLevelUp;

			[SerializeField]
			internal int _maxEnergy;

			public int Level => level;
			public float ExperienceToLevelUp => _experienceToLevelUp;
			public int MaxEnergy => _maxEnergy;
			
			public ProfileConfig Copy()
			{
				return new ProfileConfig()
				{
					_maxEnergy = MaxEnergy,
					_experienceToLevelUp = ExperienceToLevelUp,
					level = Level
				};
			}
		}
		
		public void UpdateConfigExperienceToLevelUp(int level, long experienceToLevel)
		{
			var profileConfig = GetInternal(level);

			if (profileConfig is null)
			{
				profileConfig = new ProfileConfig();
				profileConfig.level = level;
				profileConfig._maxEnergy = 30;

				if (_internalConfigs.ContainsKey(level))
				{
					_internalConfigs[level] = profileConfig;
				}
				else
				{
					_internalConfigs.Add(level, profileConfig);
				}
			}
			
			profileConfig._experienceToLevelUp = experienceToLevel;
		}
	}
}