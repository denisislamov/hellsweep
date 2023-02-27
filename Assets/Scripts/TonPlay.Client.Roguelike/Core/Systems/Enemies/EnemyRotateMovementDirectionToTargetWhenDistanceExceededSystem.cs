using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies
{
	public class EnemyRotateMovementDirectionToTargetWhenDistanceExceededSystem : IEcsInitSystem, IEcsRunSystem
	{
		public void Init(EcsSystems systems)
		{
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			
			var world = systems.GetWorld();
			var enemyFilter = world.Filter<RotateMovementDirectionToTargetWhenDistanceExceededComponent>()
								   .Inc<TargetComponent>()
								   .Inc<EnemyComponent>()
								   .Inc<PositionComponent>()
								   .Inc<MovementComponent>()
								   .End();

			var positionPool = world.GetPool<PositionComponent>();
			var movementPool = world.GetPool<MovementComponent>();
			var targetPool = world.GetPool<TargetComponent>();
			var rotateMovementPool = world.GetPool<RotateMovementDirectionToTargetWhenDistanceExceededComponent>();
			
			foreach (var entityId in enemyFilter)
			{
				ref var rotateMovement = ref rotateMovementPool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);
				ref var target = ref targetPool.Get(entityId);
				ref var targetPosition = ref positionPool.Get(target.EntityId);
				ref var movementComponent = ref movementPool.Get(entityId);

				var diffX = targetPosition.Position.x - position.Position.x;
				var diffY = targetPosition.Position.y - position.Position.y;
				var sqrMagnitude = diffX * diffX + diffY * diffY;
				
				if (sqrMagnitude < rotateMovement.Distance * rotateMovement.Distance)
				{
					movementComponent.Direction = rotateMovement.CachedDirection;
					continue;
				}
				

				var movementVector = targetPosition.Position - position.Position;
				movementVector.Normalize();

				rotateMovement.CachedDirection = movementVector;
				movementComponent.Direction = rotateMovement.CachedDirection;
			}

			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		} 
	}
}