using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class SyncRotationWithAnotherEntitySystem : IEcsRunSystem, IEcsInitSystem
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
				var filter = world.Filter<SyncRotationWithAnotherEntityComponent>().Inc<RotationComponent>().Exc<InactiveComponent>().End();

				var syncPool = world.GetPool<SyncRotationWithAnotherEntityComponent>();

				var entityRotationPool = world.GetPool<RotationComponent>();

				foreach (var entityId in filter)
				{
					ref var sync = ref syncPool.Get(entityId);
					var syncRotationPool = sync.ParentWorld.GetPool<RotationComponent>();

					if (syncRotationPool.Has(sync.ParentEntityId))
					{
						ref var rotation = ref entityRotationPool.Get(entityId);
						ref var parentRotation = ref syncRotationPool.Get(sync.ParentEntityId);
						rotation.Direction = parentRotation.Direction;
					}
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}