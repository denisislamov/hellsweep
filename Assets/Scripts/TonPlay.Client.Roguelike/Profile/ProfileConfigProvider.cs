using System;
using System.Collections.Generic;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Profile
{
	[CreateAssetMenu(fileName = nameof(ProfileConfigProvider), menuName = AssetMenuConstants.CONFIGS + nameof(ProfileConfigProvider))]
	public class ProfileConfigProvider : ScriptableObject, IProfileConfigProvider
	{
		[SerializeField]
		private List<ProfileConfig> _configs;

		public IProfileConfig Get(int level)
		{
			if (level - 1 >= _configs.Count) return null;
			
			var config = _configs[level - 1];
			config.level = level;
			
			return _configs[level - 1];
		}
		
		[Serializable]
		private class ProfileConfig : IProfileConfig
		{
			[HideInInspector]
			public int level;
			
			[SerializeField]
			private float _experienceToLevelUp;
			
			[SerializeField]
			private int _maxEnergy;

			public int Level => level;
			public float ExperienceToLevelUp => _experienceToLevelUp;
			public int MaxEnergy => _maxEnergy;
		}
	}
}