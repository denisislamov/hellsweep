using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class SyncPositionWithAnotherEntitySystem : IEcsRunSystem, IEcsInitSystem
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
				var filter = world.Filter<SyncPositionWithAnotherEntityComponent>().Inc<PositionComponent>().Exc<InactiveComponent>().End();

				var syncPool = world.GetPool<SyncPositionWithAnotherEntityComponent>();
				var entityPositionPool = world.GetPool<PositionComponent>();

				foreach (var entityId in filter)
				{
					ref var sync = ref syncPool.Get(entityId);
					var syncPositionPool = sync.ParentWorld.GetPool<PositionComponent>();

					if (syncPositionPool.Has(sync.ParentEntityId))
					{
						ref var position = ref entityPositionPool.Get(entityId);
						ref var parentPosition = ref syncPositionPool.Get(sync.ParentEntityId);
						position.Position = parentPosition.Position;
					}
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}