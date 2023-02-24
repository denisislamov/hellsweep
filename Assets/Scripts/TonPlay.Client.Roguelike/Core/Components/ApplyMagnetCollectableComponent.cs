using System.Collections.Generic;
using TonPlay.Client.Common.Utilities;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct ApplyMagnetCollectableComponent
	{
		public float TimeToMagnet;
		public SimpleIntHashSet CollectableEntityIds;
	}
}