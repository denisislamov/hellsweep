using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Roguelike.Client.Core.CollisionProcessors;
using TonPlay.Roguelike.Client.Core.CollisionProcessors.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class PlayerCollisionEventsSystem : IEcsInitSystem, IEcsRunSystem
	{
		private IReadOnlyDictionary<int, ICollisionProcessor> _layersCollisionProcessors;

		public void Init(EcsSystems systems)
		{
			var world = systems.GetWorld();
			_layersCollisionProcessors = new Dictionary<int, ICollisionProcessor>()
			{
				[LayerMask.NameToLayer("Enemy")] = new EnemyCollisionProcessor(world)
			};
		}
		
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<EventComponent>().Inc<CollisionEventComponent>().End();
			
			var usedEvents = world.GetPool<UsedEventComponent>();
			var collisions = world.GetPool<CollisionEventComponent>();

			foreach (var entityId in filter)
			{
				usedEvents.Add(entityId);

				ref var collisionComponent = ref collisions.Get(entityId);

				var collidedRigidbodyLayer = collisionComponent.CollidedRigidbody.gameObject.layer;

				if (_layersCollisionProcessors.ContainsKey(collidedRigidbodyLayer))
				{
					_layersCollisionProcessors[collidedRigidbodyLayer].Process(ref collisionComponent);
				}
					
				collisions.Del(entityId);
			}
		}
	}
}