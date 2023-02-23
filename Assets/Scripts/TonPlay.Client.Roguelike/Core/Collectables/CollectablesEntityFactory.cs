using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Collectables.Interfaces;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
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

			if (entity is null || entity.Id == EcsEntity.DEFAULT_ID || !world.IsEntityAlive(entity.Id))
			{
				return entity;
			}

			entity.AddCollisionComponent(config.CollisionAreaConfig, config.CollisionLayerMask);
			entity.AddHasCollidedComponent();
			entity.AddLayerComponent(config.Layer);

			var kdTreeStorage = _sharedData.CollectablesKdTreeStorage;

			var treeIndex = kdTreeStorage.AddEntity(entity.Id, position);

			entity.AddKdTreeElementComponent(kdTreeStorage, treeIndex);

			return entity;
		}

		public class Factory : PlaceholderFactory<ISharedData, CollectablesEntityFactory>
		{
		}
	}
}