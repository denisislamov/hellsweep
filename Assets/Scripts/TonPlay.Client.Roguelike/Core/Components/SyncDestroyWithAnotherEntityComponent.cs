using Leopotam.EcsLite;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct SyncDestroyWithAnotherEntityComponent
	{
		public int NextEntityId;
		public EcsWorld NextEntityWorld;
	}
}