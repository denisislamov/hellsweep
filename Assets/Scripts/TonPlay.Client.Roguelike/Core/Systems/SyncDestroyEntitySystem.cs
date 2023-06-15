using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class SyncDestroyEntitySystem : IEcsRunSystem, IEcsInitSystem
	{
		private EcsWorld[] _worlds;
		
		public void Init(EcsSystems systems)
		{
			_worlds = new EcsWorld[]
			{
				systems.GetWorld(),
				systems.GetWorld(RoguelikeConstants.Core.EFFECTS_WORLD_NAME)
			};
		}
		
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			for (var index = 0; index < _worlds.Length; index++)
			{
				var world = _worlds[index];
				var filter = world.Filter<DestroyComponent>().Inc<SyncDestroyWithAnotherEntityComponent>().End();
				var syncPool = world.GetPool<SyncDestroyWithAnotherEntityComponent>();
				
				foreach (var entityId in filter)
				{
					ref var sync = ref syncPool.Get(entityId);
					
					var destroyPool = sync.NextEntityWorld.GetPool<DestroyComponent>();
					destroyPool.Add(sync.NextEntityId);
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}