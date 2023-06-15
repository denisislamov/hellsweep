using Leopotam.EcsLite;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct SyncPositionWithAnotherEntityComponent
	{
		public int ParentEntityId;
		public EcsWorld ParentWorld;
	}
}