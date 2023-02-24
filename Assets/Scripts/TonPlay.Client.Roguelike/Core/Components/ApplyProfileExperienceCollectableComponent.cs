using System.Collections.Generic;
using TonPlay.Client.Common.Utilities;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct ApplyProfileExperienceCollectableComponent
	{
		public float Value;
		public SimpleIntHashSet CollectableEntityIds;
	}
}