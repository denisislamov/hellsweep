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

		private IReadOnlyDictionary<string, ILocationConfig> Map => _map ??= _configs.ToDictionary(_ => _.Id, _ => (ILocationConfig) _);

		public ILocationConfig Get(string id) => 
			!Map.ContainsKey(id) 
				? default(ILocationConfig) 
				: Map[id];

		public ILocationConfig[] Configs => _configs;
	}
}