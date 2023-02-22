using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Collectables.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Core.Collectables;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Collectables
{
	internal class BombCollectablesEntityFactory : ICollectableEntityFactory
	{
		private readonly ICompositeViewPool _pool;

		public BombCollectablesEntityFactory(
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
			entity.AddPositionComponent(position);
			entity.AddTransformComponent(gameObject.transform);
			entity.AddViewProviderComponent(gameObject);
			entity.AddPoolObjectComponent(poolObject);
			entity.AddStickToLocationBlockComponent();
			entity.AddBombCollectableComponent((IBombCollectableConfig) config);

			return entity;
		}
	}
}