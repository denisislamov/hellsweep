using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Collectables.Interfaces;
using TonPlay.Client.Roguelike.Core.Pooling.Identities;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Core.Collectables;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collectables
{
	internal class HealthCollectablesEntityFactory : ICollectableEntityFactory
	{
		private readonly ICompositeViewPool _pool;

		public HealthCollectablesEntityFactory(
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

			var entity = world.NewEntity();
			var view = poolObject.Object;

			var gameObject = view.gameObject;

			view.transform.position = position;

			entity.AddCollectableComponent(config);
			entity.AddTransformComponent(gameObject.transform);
			entity.AddPositionComponent(position);
			entity.AddViewProviderComponent(gameObject);
			entity.AddPoolObjectComponent(poolObject);
			entity.AddStickToLocationBlockComponent();
			entity.AddMagnetizableComponent();

			return entity;
		}
	}
}