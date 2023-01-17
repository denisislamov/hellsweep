using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Collectables;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class ApplyCollectablesSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world
						.Filter<ApplyCollectableComponent>()
						.End(); 
			var pool = world.GetPool<ApplyCollectableComponent>();
			var collectablePool = world.GetPool<CollectableComponent>();
			var experiencePool = world.GetPool<ApplyExperienceComponent>();

			foreach (var entityId in filter)
			{
				ref var component = ref pool.Get(entityId);
				ref var collectable = ref collectablePool.Get(component.CollectableEntityId);

				switch (collectable.Type)
				{
					case CollectableType.Experience:
						ApplyExperience(experiencePool, collectable.Value, component.AppliedEntityId, component.CollectableEntityId);
						break;
				}

				world.DelEntity(entityId);
			}
		}

		private void ApplyExperience(EcsPool<ApplyExperienceComponent> pool, float value, int entityId, int collectableEntityId)
		{
			if (pool.Has(entityId))
			{
				ref var exp = ref pool.Get(entityId);
				exp.Value += value;
				exp.CollectableEntityIds.Add(collectableEntityId);
			}
			else
			{
				ref var exp = ref pool.Add(entityId);
				exp.Value += value;
				exp.CollectableEntityIds = new HashSet<int>() {collectableEntityId};
			}
		}
	}
}