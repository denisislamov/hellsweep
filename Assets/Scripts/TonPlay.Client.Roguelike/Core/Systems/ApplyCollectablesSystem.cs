using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Collectables;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
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
			
			var profileExperiencePool = world.GetPool<ApplyProfileExperienceCollectableComponent>();
			var experiencePool = world.GetPool<ApplyExperienceCollectableComponent>();
			var healthPool = world.GetPool<ApplyHealthCollectableComponent>();
			var goldPool = world.GetPool<ApplyGoldCollectableComponent>();

			foreach (var entityId in filter)
			{
				ref var component = ref pool.Get(entityId);
				ref var collectable = ref collectablePool.Get(component.CollectableEntityId);

				switch (collectable.Type)
				{
					case CollectableType.Experience:
						ApplyExperience(experiencePool, collectable.Value, component.AppliedEntityId, component.CollectableEntityId);
						break;
					case CollectableType.Gold:
						ApplyGold(goldPool, collectable.Value, component.AppliedEntityId, component.CollectableEntityId);
						break;
					case CollectableType.ProfileExperience:
						ApplyProfileExperience(profileExperiencePool, collectable.Value, component.AppliedEntityId, component.CollectableEntityId);
						break;
					case CollectableType.Health:
						ApplyHealth(healthPool, collectable.Value, component.AppliedEntityId, component.CollectableEntityId);
						break;
				}

				world.DelEntity(entityId);
			}
		}
		
		private void ApplyHealth(EcsPool<ApplyHealthCollectableComponent> pool, float value, int entityId, int collectableEntityId)
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

		private void ApplyProfileExperience(EcsPool<ApplyProfileExperienceCollectableComponent> pool, float value, int entityId, int collectableEntityId)
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

		private void ApplyGold(EcsPool<ApplyGoldCollectableComponent> pool, float value, int entityId, int collectableEntityId)
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

		private void ApplyExperience(EcsPool<ApplyExperienceCollectableComponent> pool, float value, int entityId, int collectableEntityId)
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