using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class DestroyOnCollisionSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<HasCollidedComponent>()
							  .Inc<DestroyOnCollisionComponent>()
							  .Inc<ViewProviderComponent>()
							  .Exc<InactiveComponent>().
							   End();

			var viewPool = world.GetPool<ViewProviderComponent>();
			var inactivePool = world.GetPool<InactiveComponent>();

			foreach (var entityId in filter)
			{
				ref var view = ref viewPool.Get(entityId);
				view.View.SetActive(false);

				inactivePool.Add(entityId);
			}
		}
	}
}