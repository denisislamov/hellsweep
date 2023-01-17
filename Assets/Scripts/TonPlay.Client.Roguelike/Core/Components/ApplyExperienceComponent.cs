using System.Collections.Generic;

namespace TonPlay.Roguelike.Client.Core.Components
{
	public struct ApplyExperienceComponent
	{
		public float Value;
		public HashSet<int> CollectableEntityIds;
	}
}