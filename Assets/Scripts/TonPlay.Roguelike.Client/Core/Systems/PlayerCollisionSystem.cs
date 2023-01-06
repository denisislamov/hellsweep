using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.CollisionProcessors;
using TonPlay.Roguelike.Client.Core.CollisionProcessors.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class PlayerCollisionSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly Collider2D[] _overlappedColliders = new Collider2D[128];

		private int _overlapLayerMask;
		private IReadOnlyDictionary<int, ICollisionProcessor> _layersCollisionProcessors;
		
		public void Init(EcsSystems systems)
		{
			var world = systems.GetWorld();
			_overlapLayerMask = LayerMask.GetMask("Enemy", "Utility");
			
			_layersCollisionProcessors = new Dictionary<int, ICollisionProcessor>()
			{
				[LayerMask.NameToLayer("Enemy")] = new EnemyToPlayerCollisionProcessor(world, systems.GetShared<SharedData>())
			};
		}
		
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<PlayerComponent>().Inc<PositionComponent>().Exc<DeadComponent>().End();
			var positionComponents = world.GetPool<PositionComponent>();
			
			foreach (var entityId in filter)
			{
				ref var rigidbodyComponent = ref positionComponents.Get(entityId);
				var collisionsCount = Physics2D.OverlapCircleNonAlloc(
					rigidbodyComponent.Position, 
					1f, 
					_overlappedColliders,
					_overlapLayerMask);

				for (var i = 0; i < collisionsCount; i++)
				{
					var overlappedCollider = _overlappedColliders[i];
					var collidedRigidbodyLayer = overlappedCollider.gameObject.layer;

					if (_layersCollisionProcessors.ContainsKey(collidedRigidbodyLayer))
					{
						_layersCollisionProcessors[collidedRigidbodyLayer].Process(ref overlappedCollider);
					}
				}
			}
		}
	}
}