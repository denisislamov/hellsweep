using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Collectables.Interfaces;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Roguelike.Client.Core;
using TonPlay.Roguelike.Client.Core.Collectables;
using UnityEngine;
using Zenject;

namespace TonPlay.Client.Roguelike.Core.Collectables
{
	internal class CollectablesEntityFactory : ICollectableEntityFactory
	{
		private readonly IReadOnlyDictionary<CollectableType, ICollectableEntityFactory> _factories;
		private readonly ISharedData _sharedData;

		public CollectablesEntityFactory(
			ISharedData sharedData,
			BombCollectablesEntityFactory bombCollectablesEntityFactory,
			GoldCollectablesEntityFactory goldCollectablesEntityFactory,
			HealthCollectablesEntityFactory healthCollectablesEntityFactory,
			MagnetCollectablesEntityFactory magnetCollectablesEntityFactory,
			ExperienceCollectablesEntityFactory experienceCollectablesEntityFactory,
			ProfileExperienceCollectablesEntityFactory profileExperienceCollectablesEntityFactory)
		{
			_sharedData = sharedData;
			_factories = new Dictionary<CollectableType, ICollectableEntityFactory>()
			{
				[CollectableType.Bomb] = bombCollectablesEntityFactory,
				[CollectableType.Gold] = goldCollectablesEntityFactory,
				[CollectableType.Health] = healthCollectablesEntityFactory,
				[CollectableType.Magnet] = magnetCollectablesEntityFactory,
				[CollectableType.Experience] = experienceCollectablesEntityFactory,
				[CollectableType.ProfileExperience] = profileExperienceCollectablesEntityFactory
			};
		}

		public EcsEntity Create(EcsWorld world, ICollectableConfig config, Vector2 position)
		{
			var entity = _factories[config.Type].Create(world, config, position);
			
			if (entity is null || entity.Id == EcsEntity.DEFAULT_ID)
			{
				return entity;
			}

			var kdTreeStorage = _sharedData.CollectablesKdTreeStorage;
			var freeTreeIndex = FindFreeTreeIndex(kdTreeStorage);

			kdTreeStorage.KdTreeEntityIdToPositionIndexMap.Add(entity.Id, freeTreeIndex);
			kdTreeStorage.KdTreePositionIndexToEntityIdMap[freeTreeIndex] = entity.Id;
			kdTreeStorage.KdTree.Points[freeTreeIndex] = position;

			return entity;
		}

		private int FindFreeTreeIndex(KdTreeStorage kdTreeStorage)
		{
			var freeTreeIndex = -1;
			for (var i = 0; i < kdTreeStorage.KdTreePositionIndexToEntityIdMap.Length; i++)
			{
				if (kdTreeStorage.KdTreeEntityIdToPositionIndexMap.ContainsKey(kdTreeStorage.KdTreePositionIndexToEntityIdMap[i]))
				{
					continue;
				}

				freeTreeIndex = i;

				break;
			}
			return freeTreeIndex;
		}

		public class Factory : PlaceholderFactory<ISharedData, CollectablesEntityFactory>
		{
		}
	}
}