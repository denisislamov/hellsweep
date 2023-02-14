using System;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(WeaponConfigProvider), menuName = AssetMenuConstants.CORE_CONFIGS + nameof(WeaponConfigProvider))]
	public class WeaponConfigProvider : ScriptableObject, IWeaponConfigProvider
	{
		[SerializeField]
		private WeaponConfig[] _configs;
		
		public IWeaponConfig Get(string id)
		{
			return _configs.First(config => config.ItemId == id);
		}
		
		[Serializable]
		private class WeaponConfig : IWeaponConfig
		{
			[SerializeField]
			private string _itemId;
		
			[SerializeField]
			private SkillName _skillName;

			public string ItemId => _itemId;
			public SkillName SkillName => _skillName;
		}
	}
}