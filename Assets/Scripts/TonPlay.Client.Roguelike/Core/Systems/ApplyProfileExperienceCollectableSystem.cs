using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ApplyProfileExperienceCollectableSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world
						.Filter<ApplyProfileExperienceCollectableComponent>()
						.Inc<ProfileExperienceComponent>()
						.Exc<DeadComponent>()
						.End();
			var applyPool = world.GetPool<ApplyProfileExperienceCollectableComponent>();
			var expPool = world.GetPool<ProfileExperienceComponent>();
			var destroyPool = world.GetPool<DestroyComponent>();

			foreach (var entityId in filter)
			{
				ref var apply = ref applyPool.Get(entityId);
				
				if (apply.CollectableEntityIds.Count <= 0)
				{
					continue;
				}
				
				ref var profileExp = ref expPool.Get(entityId);

				profileExp.Value += apply.Value;

				foreach (var collectableEntityId in apply.CollectableEntityIds)
				{
					destroyPool.Add(collectableEntityId);
				}

				apply.CollectableEntityIds.Clear();
				apply.Value = 0f;
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}