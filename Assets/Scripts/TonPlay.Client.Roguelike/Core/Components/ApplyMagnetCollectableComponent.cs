using System.Collections.Generic;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct ApplyMagnetCollectableComponent
	{
		public float TimeToMagnet;
		public HashSet<int> CollectableEntityIds;
	}
}