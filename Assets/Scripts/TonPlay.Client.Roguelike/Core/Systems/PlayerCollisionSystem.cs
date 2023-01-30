using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Roguelike.Client.Core.Collision;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class PlayerCollisionSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly IOverlapExecutor _overlapExecutor;

		private List<int> _overlappedEntities = new List<int>(32);

		private int _overlapLayerMask;
		private IReadOnlyDictionary<int, ICollisionProcessor> _layersCollisionProcessors;

		private KDQuery _query = new KDQuery();

		public PlayerCollisionSystem(IOverlapExecutor overlapExecutor)
		{
			_overlapExecutor = overlapExecutor;
		}

		public void Init(EcsSystems systems)
		{
			var world = systems.GetWorld();
			
			_overlapLayerMask = LayerMask.GetMask("Enemy", "Utility");
			
			_layersCollisionProcessors = new Dictionary<int, ICollisionProcessor>()
			{
				[LayerMask.NameToLayer("Enemy")] = new PlayerWithEnemyCollisionProcessor(world),
				[LayerMask.NameToLayer("Utility")] = new PlayerWithUtilityCollisionProcessor(world)
			};
		}
		
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<ISharedData>();
			var filter = world.Filter<PlayerComponent>().Inc<PositionComponent>().Exc<DeadComponent>().End();
			var positionComponents = world.GetPool<PositionComponent>();
			var playerComponents = world.GetPool<PlayerComponent>();
			var layerComponents = world.GetPool<LayerComponent>();
			
			foreach (var playerEntityId in filter)
			{
				ref var positionComponent = ref positionComponents.Get(playerEntityId);
				ref var playerComponent = ref playerComponents.Get(playerEntityId);

				var collisionAreaConfig = sharedData.PlayerConfigProvider.Get(playerComponent.ConfigId).CollisionAreaConfig;
				
				var collisionsCount = _overlapExecutor.Overlap(
					_query,
					positionComponent.Position, 
					collisionAreaConfig,
					ref _overlappedEntities,
					_overlapLayerMask);
				
				for (var i = 0; i < collisionsCount; i++)
				{
					var overlappedEntityId = _overlappedEntities[i];

					if (!layerComponents.Has(overlappedEntityId))
					{
						Debug.LogWarning($"{overlappedEntityId} doesn't have {nameof(LayerComponent)}");
						continue;
					}
					
					ref var collidedRigidbodyLayer = ref layerComponents.Get(overlappedEntityId);

					if (_layersCollisionProcessors.ContainsKey(collidedRigidbodyLayer.Layer))
					{
						_layersCollisionProcessors[collidedRigidbodyLayer.Layer].Process(ref overlappedEntityId);
					}
				}
				
				_overlappedEntities.Clear();
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}