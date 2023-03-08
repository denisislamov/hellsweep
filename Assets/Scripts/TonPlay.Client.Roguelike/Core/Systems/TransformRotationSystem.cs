using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class TransformRotationSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world.Filter<RotationComponent>()
							  .Inc<TransformComponent>()
							  .Exc<IgnoreTransformRotation>()
							  .Exc<RigidbodyComponent>()
							  .Exc<DeadComponent>()
							  .Exc<InactiveComponent>()
							  .End();
			var rotationComponents = world.GetPool<RotationComponent>();
			var transformComponents = world.GetPool<TransformComponent>();

			foreach (var entityId in filter)
			{
				ref var rotation = ref rotationComponents.Get(entityId);
				ref var transform = ref transformComponents.Get(entityId);

				transform.Transform.right = rotation.Direction;
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}