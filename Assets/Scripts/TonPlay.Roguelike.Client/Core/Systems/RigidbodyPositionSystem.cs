using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class RigidbodyPositionSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var rigidbodyComponents = world.GetPool<RigidbodyComponent>();
			var positionComponents = world.GetPool<PositionComponent>();
			var filter = world.Filter<RigidbodyComponent>()
							  .Exc<DeadComponent>()
							  .Exc<InactiveComponent>()
							  .End();

			foreach (var entityId in filter)
			{
				ref var rigidbodyComponent = ref rigidbodyComponents.Get(entityId);
				
				if (positionComponents.Has(entityId))
				{
					ref var positionComponent = ref positionComponents.Get(entityId);
					rigidbodyComponent.Rigidbody.MovePosition(positionComponent.Position);
					positionComponent.Position = rigidbodyComponent.Rigidbody.position;
				}
				else
				{
					ref var positionComponent = ref positionComponents.Add(entityId);
					positionComponent.Position = rigidbodyComponent.Rigidbody.position;
				}
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}