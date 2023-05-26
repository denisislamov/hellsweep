using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Collectables.Interfaces;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Client.Roguelike.Utilities;
using TonPlay.Roguelike.Client.Core.Collectables;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collectables
{
	internal class ExperienceCollectablesEntityFactory : ICollectableEntityFactory
	{
		private readonly ICompositeViewPool _pool;

		public ExperienceCollectablesEntityFactory(
			ICompositeViewPool pool)
		{
			_pool = pool;
		}

		public EcsEntity Create(EcsWorld world, ICollectableConfig config, Vector2 position)
		{
			if (!_pool.TryGet<CollectableView>(new CollectableViewPoolIdentity(config.Prefab), out var poolObject))
			{
				return null;
			}
			
			var view = poolObject.Object;
			var entity = world.NewEntity();

			if (poolObject.EntityId != EcsEntity.DEFAULT_ID)
			{
				world.DelEntity(poolObject.EntityId);
				
				poolObject.EntityId = entity.Id;
			}

			var gameObject = view.gameObject;

			view.transform.position = position;

			entity.AddCollectableComponent(config);
			entity.AddPositionComponent(position);
			entity.AddTransformComponent(gameObject.transform);
			entity.AddViewProviderComponent(gameObject);
			entity.AddPoolObjectComponent(poolObject);
			entity.AddMagnetizableComponent();

			return entity;
		}
	}
}