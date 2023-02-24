using TonPlay.Client.Common.Utilities;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct ApplyExperienceCollectableComponent
	{
		public float Value;
		public SimpleIntHashSet CollectableEntityIds;
	}
}