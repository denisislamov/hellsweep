using System;
using System.Linq;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Views;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Weapons.Configs
{
	[CreateAssetMenu(fileName = nameof(WeaponConfigProvider), menuName = AssetMenuConstants.CORE_CONFIGS + nameof(WeaponConfigProvider))]
	public class WeaponConfigProvider : ScriptableObject, IWeaponConfigProvider
	{
		[SerializeField]
		private WeaponConfig[] _configs;
		
		public IWeaponConfig Get(string id)
		{
			return _configs.First(config => config.Id == id);
		}
		
		[Serializable]
		private class WeaponConfig : IWeaponConfig
		{
			[SerializeField]
			private string _id;
		
			[SerializeField]
			private WeaponView _prefab;

			[SerializeField]
			private ProjectileConfig _projectileConfig;
			
			[SerializeField]
			private float _fireDelay;
			
			[SerializeField]
			private WeaponFireType _fireType;

			public string Id => _id;
		
			public WeaponView Prefab => _prefab;
			public float FireDelay => _fireDelay;
			public WeaponFireType FireType => _fireType;

			public IProjectileConfig GetProjectileConfig() => _projectileConfig;
		}
	}
}