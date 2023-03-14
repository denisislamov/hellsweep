using System;
using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Drops;
using TonPlay.Client.Roguelike.Core.Drops.Interfaces;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Interfaces;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties;
using TonPlay.Client.Roguelike.Core.Enemies.Configs.Properties.Interfaces;
using TonPlay.Client.Roguelike.Core.Enemies.Views;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Enemies.Configs
{
	[CreateAssetMenu(fileName = nameof(EnemyConfig), menuName = AssetMenuConstants.ENEMIES_CONFIGS + nameof(EnemyConfig))]
	internal class EnemyConfig : ScriptableObject, IEnemyConfig
	{
		[SerializeField]
		private string _id;

		[SerializeField]
		private EnemyType _enemyType;

		[SerializeField]
		private EnemyView _prefab;

		[SerializeField]
		private WeightedCollectableIdDropConfig[] _collectablesIdsOnDeath;

		[SerializeField]
		private EnemyPropertyConfig[] _propertyConfigs;

		private IItemDrop<string>[] _randomCollectablesDrop;
		private Dictionary<Type, IEnemyPropertyConfig> _enemyPropertyConfigMap;
		private IViewPoolIdentity _identity;

		public string Id => _id;
		public EnemyType EnemyType => _enemyType;

		public EnemyView Prefab => _prefab;

		public IItemDrop<string>[] RandomCollectableDrops => _randomCollectablesDrop ??= _collectablesIdsOnDeath.Select(_ => new RandomCollectableIdDrop(_)).ToArray();
		public IViewPoolIdentity Identity => _identity ??= new EnemyViewPoolIdentity(Prefab);

		public bool HasProperty<T>() where T : IEnemyPropertyConfig
		{
			if (_propertyConfigs is null)
			{
				return false;
			}

			for (var i = 0; i < _propertyConfigs.Length; i++)
			{
				var propertyConfig = _propertyConfigs[i];

				if (propertyConfig is T typedConfig)
				{
					return true;
				}
			}

			return false;
		}

		public T GetProperty<T>() where T : IEnemyPropertyConfig
		{
			if (_propertyConfigs is null)
			{
				return default(T);
			}

			for (var i = 0; i < _propertyConfigs.Length; i++)
			{
				var propertyConfig = _propertyConfigs[i];

				if (propertyConfig is T typedConfig)
				{
					return typedConfig;
				}
			}

			return default(T);
		}
	}
}