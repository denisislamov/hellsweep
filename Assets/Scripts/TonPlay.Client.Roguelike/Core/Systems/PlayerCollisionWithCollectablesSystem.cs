using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class PlayerCollisionWithCollectablesSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly IOverlapExecutor _overlapExecutor;

		private List<int> _overlappedEntities = new List<int>(32);

		private int _overlapLayerMask;

		private KDQuery _query = new KDQuery();
		private PlayerWithUtilityCollisionProcessor _collisionProcessor;

		public PlayerCollisionWithCollectablesSystem(IOverlapExecutor overlapExecutor)
		{
			_overlapExecutor = overlapExecutor;
		}

		public void Init(EcsSystems systems)
		{
			var world = systems.GetWorld();

			_overlapLayerMask = LayerMask.GetMask("Utility");
			_collisionProcessor = new PlayerWithUtilityCollisionProcessor(world);
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world.Filter<PlayerComponent>()
							  .Inc<PositionComponent>()
							  .Inc<CollisionAreaWithCollectablesComponent>()
							  .Exc<DeadComponent>()
							  .End();
			var collisionAreaWithCollectablesComponents = world.GetPool<CollisionAreaWithCollectablesComponent>();
			var positionComponents = world.GetPool<PositionComponent>();

			var overlapParams = OverlapParams.Create(world);
			overlapParams.SetFilter(overlapParams.CreateDefaultFilterMask().End());
			overlapParams.Build();

			foreach (var playerEntityId in filter)
			{
				ref var collisionAreaWithCollectablesComponent = ref collisionAreaWithCollectablesComponents.Get(playerEntityId);
				ref var positionComponent = ref positionComponents.Get(playerEntityId);
				
				var collisionsCount = _overlapExecutor.Overlap(
					_query,
					positionComponent.Position,
					collisionAreaWithCollectablesComponent.CollisionArea,
					ref _overlappedEntities,
					_overlapLayerMask,
					overlapParams);

				for (var i = 0; i < collisionsCount; i++)
				{
					var overlappedEntityId = _overlappedEntities[i];

					_collisionProcessor.Process(ref overlappedEntityId);
				}

				_overlappedEntities.Clear();
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}