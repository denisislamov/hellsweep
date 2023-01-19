using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct ApplyGoldCollectableComponent
	{
		public float Value;
		public HashSet<int> CollectableEntityIds;
	}
}