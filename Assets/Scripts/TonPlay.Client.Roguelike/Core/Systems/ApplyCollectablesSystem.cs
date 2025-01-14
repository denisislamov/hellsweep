using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Collectables;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ApplyCollectablesSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world
						.Filter<ApplyCollectableComponent>()
						.End();
			var pool = world.GetPool<ApplyCollectableComponent>();
			var collectablePool = world.GetPool<CollectableComponent>();

			var applyProfileExperiencePool = world.GetPool<ApplyProfileExperienceCollectableComponent>();
			var applyExperiencePool = world.GetPool<ApplyExperienceCollectableComponent>();
			var applyHealthPool = world.GetPool<ApplyHealthCollectableComponent>();
			var applyMagnetPool = world.GetPool<ApplyMagnetCollectableComponent>();
			var applyGoldPool = world.GetPool<ApplyGoldCollectableComponent>();
			var applyBombPool = world.GetPool<ApplyBombCollectableComponent>();

			foreach (var entityId in filter)
			{
				ref var component = ref pool.Get(entityId);
				ref var collectable = ref collectablePool.Get(component.CollectableEntityId);

				switch (collectable.Type)
				{
					case CollectableType.Experience:
						ApplyExperience(applyExperiencePool, collectable.Value, component.AppliedEntityId, component.CollectableEntityId);
						break;
					case CollectableType.Gold:
						ApplyGold(applyGoldPool, collectable.Value, component.AppliedEntityId, component.CollectableEntityId);
						break;
					case CollectableType.ProfileExperience:
						ApplyProfileExperience(applyProfileExperiencePool, collectable.Value, component.AppliedEntityId, component.CollectableEntityId);
						break;
					case CollectableType.Health:
						ApplyHealth(applyHealthPool, collectable.Value, component.AppliedEntityId, component.CollectableEntityId);
						break;
					case CollectableType.Magnet:
						ApplyMagnet(applyMagnetPool, component.AppliedEntityId, component.CollectableEntityId);
						break;
					case CollectableType.Bomb:
						ApplyBomb(applyBombPool, component.AppliedEntityId, component.CollectableEntityId);
						break;
				}

				world.DelEntity(entityId);
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
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
				exp.CollectableEntityIds = new SimpleIntHashSet(8);
				exp.CollectableEntityIds.Add(collectableEntityId);
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
				exp.CollectableEntityIds = new SimpleIntHashSet(8);
				exp.CollectableEntityIds.Add(collectableEntityId);
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
				exp.CollectableEntityIds = new SimpleIntHashSet(8);
				exp.CollectableEntityIds.Add(collectableEntityId);
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
				exp.CollectableEntityIds = new SimpleIntHashSet(32);
				exp.CollectableEntityIds.Add(collectableEntityId);
			}
		}

		private void ApplyMagnet(
			EcsPool<ApplyMagnetCollectableComponent> applyPool,
			int entityId,
			int collectableEntityId)
		{
			if (applyPool.Has(entityId))
			{
				ref var exp = ref applyPool.Get(entityId);
				exp.CollectableEntityIds.Add(collectableEntityId);
			}
			else
			{
				ref var exp = ref applyPool.Add(entityId);
				exp.CollectableEntityIds = new SimpleIntHashSet();
				exp.CollectableEntityIds.Add(collectableEntityId);
			}
		}

		private void ApplyBomb(
			EcsPool<ApplyBombCollectableComponent> applyPool,
			int entityId,
			int collectableEntityId)
		{
			if (applyPool.Has(entityId))
			{
				ref var exp = ref applyPool.Get(entityId);
				exp.CollectableEntityIds.Add(collectableEntityId);
			}
			else
			{
				ref var exp = ref applyPool.Add(entityId);
				exp.CollectableEntityIds = new SimpleIntHashSet();
				exp.CollectableEntityIds.Add(collectableEntityId);
			}
		}
	}
}