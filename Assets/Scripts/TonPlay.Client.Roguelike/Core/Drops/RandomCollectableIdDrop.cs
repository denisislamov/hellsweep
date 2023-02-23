using System.Collections.Generic;
using System.Linq;
using TonPlay.Client.Roguelike.Core.Drops.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Drops
{
	internal class RandomCollectableIdDrop : IItemDrop<string>
	{
		private readonly WeightedCollectableIdDropConfig _config;
		public RandomCollectableIdDrop(WeightedCollectableIdDropConfig config)
		{
			_config = config;
		}

		public string Drop()
		{
			var random = Random.Range(0, 1f);
			return random <= _config.Weight ? _config.CollectableId : null;
		}
	}
}