using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class TransformRotationSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world.Filter<RotationComponent>()
							  .Inc<TransformComponent>()
							  .Exc<RigidbodyComponent>()
							  .Exc<DeadComponent>()
							  .Exc<InactiveComponent>()
							  .End();
			var rotationComponents = world.GetPool<RotationComponent>();
			var transformComponents = world.GetPool<TransformComponent>();

			foreach (var entityId in filter) {
				ref var rotation = ref rotationComponents.Get(entityId);
				ref var transform = ref transformComponents.Get(entityId);

				transform.Transform.right = rotation.Direction;
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}