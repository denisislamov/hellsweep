using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class SyncRotationWithAnotherEntitySystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();

			var filter = world.Filter<SyncRotationWithAnotherEntityComponent>().Inc<RotationComponent>().Exc<InactiveComponent>().End();
			
			var syncPool = world.GetPool<SyncRotationWithAnotherEntityComponent>();
			var rotationPool = world.GetPool<RotationComponent>();

			foreach (var entityId in filter)
			{
				ref var sync = ref syncPool.Get(entityId);
				
				if (rotationPool.Has(sync.ParentEntityId))
				{
					ref var rotation = ref rotationPool.Get(entityId);
					ref var parentRotation = ref rotationPool.Get(sync.ParentEntityId);
					rotation.Direction = parentRotation.Direction;
				}
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}