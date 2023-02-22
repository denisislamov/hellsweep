using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class RigidbodyPositionSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var rigidbodyPool = world.GetPool<RigidbodyComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			
			var filter = world.Filter<RigidbodyComponent>()
							  .Exc<DeadComponent>()
							  .Exc<InactiveComponent>()
							  .End();

			foreach (var entityId in filter)
			{
				ref var rigidbodyComponent = ref rigidbodyPool.Get(entityId);
				ref var positionComponent = ref positionPool.AddOrGet(entityId);

				var position = rigidbodyComponent.Rigidbody.position;
				
				positionComponent.Position = position;
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}