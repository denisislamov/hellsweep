using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class TransformPositionSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var transformPool = world.GetPool<TransformComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var moveInLocalSpacePool = world.GetPool<MoveInLocalSpaceOfEntityComponent>();
			var localPositionPool = world.GetPool<LocalPositionComponent>();

			var filter = world.Filter<TransformComponent>()
							  .Exc<RigidbodyComponent>()
							  .Exc<CameraComponent>()
							  .Exc<DeadComponent>()
							  .Exc<InactiveComponent>()
							  .End();

			foreach (var entityId in filter)
			{
				ref var transformComponent = ref transformPool.Get(entityId);
				ref var positionComponent = ref positionPool.AddOrGet(entityId);
				
				if (!positionPool.Has(entityId))
				{
					positionComponent.Position = transformComponent.Transform.position;
				}
				else
				{
					if (moveInLocalSpacePool.Has(entityId) && localPositionPool.Has(entityId))
					{
						ref var moveInLocalSpaceComponent = ref moveInLocalSpacePool.Get(entityId);
						ref var localPositionComponent = ref localPositionPool.Get(entityId);
						ref var parentPositionComponent = ref positionPool.Get(moveInLocalSpaceComponent.EntityId);

						positionComponent.Position = parentPositionComponent.Position + localPositionComponent.Position;
					}
					
					transformComponent.Transform.position = positionComponent.Position;
				}
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}