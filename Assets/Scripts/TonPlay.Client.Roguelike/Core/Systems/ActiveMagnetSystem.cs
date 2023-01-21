using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Leopotam.EcsLite;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;
using TonPlay.Roguelike.Client.Core;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ActiveMagnetSystem : IEcsRunSystem
	{
		private const float MAGNET_SPEED_PER_SEC = 10f;

		private readonly List<int> _nearCollectables = new List<int>();
		private readonly KdTreeStorage _kdTreeStorage;

		public ActiveMagnetSystem(KdTreeStorage kdTreeStorage)
		{
			_kdTreeStorage = kdTreeStorage;
		}

		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();

			var filter = world
						.Filter<ActiveMagnetComponent>()
						.Exc<DeadComponent>()
						.End();

			var magnetizableCollectableFilter = world
											   .Filter<CollectableComponent>()
											   .Inc<MagnetizableComponent>()
											   .Inc<TransformComponent>()
											   .Exc<InactiveComponent>()
											   .Exc<MagnetisedToEntityComponent>()
											   .Exc<DestroyComponent>()
											   .End();

			var magnetizableCollectableRawEntitiesArray = magnetizableCollectableFilter.GetRawEntities();
			var magnetizableCollectableHashSet = magnetizableCollectableRawEntitiesArray.ToHashSet();

			var positionPool = world.GetPool<PositionComponent>();
			var magnetToEntityPool = world.GetPool<MagnetisedToEntityComponent>();
			var activeMagnetPool = world.GetPool<ActiveMagnetComponent>();
			var destroyPool = world.GetPool<DestroyComponent>();
			var stickEaseMovementToEntityPool = world.GetPool<StickEaseMovementToEntityPositionComponent>();
			var easeMovementPool = world.GetPool<EaseMovementComponent>();

			foreach (var entityId in filter)
			{
				ref var activeMagnet = ref activeMagnetPool.Get(entityId);

				magnetizableCollectableHashSet.ExceptWith(activeMagnet.ExcludeEntityIds);
				
				MagnetNearCollectables(
					entityId, 
					magnetizableCollectableHashSet, 
					magnetToEntityPool, 
					positionPool, 
					destroyPool,
					stickEaseMovementToEntityPool,
					easeMovementPool,
					ref activeMagnet);
				
				magnetizableCollectableHashSet.UnionWith(activeMagnet.ExcludeEntityIds);

				activeMagnet.TimeLeft -= Time.deltaTime;
				
				if (activeMagnet.TimeLeft <= 0)
				{
					activeMagnetPool.Del(entityId);
				}
			}
		}
		private void MagnetNearCollectables(
			int appliedEntityId,
			ICollection<int> magnetizableCollectableCollection,
			EcsPool<MagnetisedToEntityComponent> magnetToEntityPool,
			EcsPool<PositionComponent> positionPool,
			EcsPool<DestroyComponent> destroyPool,
			EcsPool<StickEaseMovementToEntityPositionComponent> stickEaseMovementToEntityPool,
			EcsPool<EaseMovementComponent> easeMovementPool,
			ref ActiveMagnetComponent activeMagnet)
		{
			ref var position = ref positionPool.Get(appliedEntityId);

			_kdTreeStorage.KdQuery.Radius(_kdTreeStorage.KdTree, position.Position, activeMagnet.MagnetRadius, _nearCollectables);

			foreach (var nearCollectableTreeIndex in _nearCollectables)
			{
				var nearCollectableEntityId = _kdTreeStorage.KdTreePositionIndexToEntityIdMap[nearCollectableTreeIndex];

				if (!magnetizableCollectableCollection.Contains(nearCollectableEntityId) || magnetToEntityPool.Has(nearCollectableEntityId))
				{
					continue;
				}

				ref var nearCollectablePosition = ref positionPool.Get(nearCollectableEntityId);

				ref var magnetToEntity = ref magnetToEntityPool.Add(nearCollectableEntityId);
				magnetToEntity.EntityId = appliedEntityId;

				var time = Vector2.Distance(nearCollectablePosition.Position, position.Position)/MAGNET_SPEED_PER_SEC;

				ref var stick = ref stickEaseMovementToEntityPool.Add(nearCollectableEntityId);
				ref var easeMovement = ref easeMovementPool.Add(nearCollectableEntityId);

				stick.EntityId = appliedEntityId;
				easeMovement.Ease = Ease.InCirc;
				easeMovement.FromPosition = nearCollectablePosition.Position;
				easeMovement.ToPosition = position.Position;
				easeMovement.Duration = time;
			}

			_nearCollectables.Clear();
		}
	}
}