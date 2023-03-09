using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collectables.Config
{
	[CreateAssetMenu(fileName = nameof(CollectableConfigProvider), menuName = AssetMenuConstants.COLLECTABLES_CONFIGS + nameof(CollectableConfigProvider))]
	public class CollectableConfigProvider : ScriptableObject, ICollectableConfigProvider
	{
		[SerializeField]
		private CollectableConfig[] _configs;

		[SerializeField]
		private CollectableConfig _initialExperienceCollectableForFirstLevel;

		private Dictionary<string, ICollectableConfig> _map;

		public ICollectableConfig InitialExperienceCollectableForFirstLevel => _initialExperienceCollectableForFirstLevel;
		public IEnumerable<ICollectableConfig> AllCollectables => _configs;

		private Dictionary<string, ICollectableConfig> Map => _map ??= _configs.ToDictionary(_ => _.Id, _ => (ICollectableConfig)_);

		public ICollectableConfig Get(string id)
		{
			return Map.ContainsKey(id) ? Map[id] : null;
		}
	}
}