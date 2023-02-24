using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class EnemyMovementToTargetSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly IOverlapExecutor _overlapExecutor;

		public void Init(EcsSystems systems)
		{
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			
			var world = systems.GetWorld();
			var enemyFilter = world.Filter<MoveToTargetComponent>()
								   .Inc<EnemyTargetComponent>()
								   .Inc<EnemyComponent>()
								   .Inc<PositionComponent>()
								   .Inc<SpeedComponent>()
								   .Inc<MovementComponent>()
								   .End();

			var positionPool = world.GetPool<PositionComponent>();
			var movementPool = world.GetPool<MovementComponent>();
			var targetPool = world.GetPool<EnemyTargetComponent>();

			foreach (var entityId in enemyFilter)
			{
				ref var target = ref targetPool.Get(entityId);
				ref var targetPosition = ref positionPool.Get(target.EntityId);
				ref var movementComponent = ref movementPool.Get(entityId);
				ref var rigidbodyComponent = ref positionPool.Get(entityId);

				var position = rigidbodyComponent.Position;
				var movementVector = targetPosition.Position - position;
				movementVector.Normalize();

				movementComponent.Direction = movementVector;
			}

			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		} 
	}
}