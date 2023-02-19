using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ReduceApplyForceSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<ApplyForceComponent>().Inc<ForcibleComponent>().End();
			var forciblePool = world.GetPool<ForcibleComponent>();
			var applyForcePool = world.GetPool<ApplyForceComponent>();

			foreach (var entityId in filter)
			{
				ref var forcible = ref forciblePool.Get(entityId);
				ref var applyForce = ref applyForcePool.Get(entityId);

				applyForce.Force -= applyForce.Force * (forcible.ReduceForceRate * Time.deltaTime);
			}
		}
	}
}