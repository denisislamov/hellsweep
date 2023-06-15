using Leopotam.EcsLite;

namespace TonPlay.Client.Roguelike.Core.Components
{
	public struct SyncRotationWithAnotherEntityComponent
	{
		public int ParentEntityId;
		public EcsWorld ParentWorld;
	}
}