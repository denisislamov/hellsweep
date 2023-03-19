using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Locations.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Locations
{
	[CreateAssetMenu(fileName = nameof(LocationConfigProvider), menuName = AssetMenuConstants.LOCATION_CONFIGS + nameof(LocationConfigProvider))]
	public class LocationConfigProvider : ScriptableObject, ILocationConfigProvider
	{
		[SerializeField]
		private LocationConfig[] _configs;

		private Dictionary<string, ILocationConfig> _map;

		private IReadOnlyDictionary<string, ILocationConfig> Map
		{
			get
			{
				if (_map == null)
				{
					for (var index = 0; index < _configs.Length; index++)
					{
						var locationConfig = _configs[index];
						locationConfig.index = index;
					}

					_map = _configs.ToDictionary(_ => _.Id, _ => (ILocationConfig)_);
				}
				return _map;
			}
		}

		public ILocationConfig Get(string id) =>
			!Map.ContainsKey(id)
				? default(ILocationConfig)
				: Map[id];

		public ILocationConfig Get(int index) =>
			index >= _configs.Length || index < 0
				? default
				: _configs[index];

		public ILocationConfig[] Configs => _configs;
	}
}