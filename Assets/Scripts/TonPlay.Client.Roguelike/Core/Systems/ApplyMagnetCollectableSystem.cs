using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using TonPlay.Client.Common.Utilities;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ApplyMagnetCollectableSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world
						.Filter<ApplyMagnetCollectableComponent>()
						.Inc<PositionComponent>()
						.Exc<DeadComponent>()
						.End();

			var applyPool = world.GetPool<ApplyCollectableComponent>();
			var applyMagnetPool = world.GetPool<ApplyMagnetCollectableComponent>();
			var destroyPool = world.GetPool<DestroyComponent>();
			var activeMagnetPool = world.GetPool<ActiveMagnetComponent>();
			var magnetCollectablePool = world.GetPool<MagnetCollectableComponent>();

			var magnetCollectableHashSet = world
										  .Filter<MagnetCollectableComponent>()
										  .Exc<InactiveComponent>()
										  .Exc<DestroyComponent>()
										  .End()
										  .GetRawEntities()
										  .ToHashSet();

			foreach (var appliedEntityId in filter)
			{
				ref var apply = ref applyMagnetPool.Get(appliedEntityId);

				var collectableEntityIds = apply.CollectableEntityIds.ToStack();

				while (collectableEntityIds.Count > 0)
				{
					var collectableEntityId = collectableEntityIds.Pop();
					
					if (!magnetCollectableHashSet.Contains(collectableEntityId) || 
						destroyPool.Has(collectableEntityId) || 
						!magnetCollectablePool.Has(collectableEntityId))
					{
						continue;
					}

					destroyPool.Add(collectableEntityId);

					var config = magnetCollectablePool.Get(collectableEntityId).Config;

					if (activeMagnetPool.Has(appliedEntityId))
					{
						ref var activeMagnet = ref activeMagnetPool.Get(appliedEntityId);
						activeMagnet.TimeLeft += config.ActiveTime;
						activeMagnet.ExcludeEntityIds.Add(collectableEntityId);
					}
					else
					{
						ref var activeMagnet = ref activeMagnetPool.Add(appliedEntityId);
						activeMagnet.TimeLeft += config.ActiveTime;
						activeMagnet.MagnetRadius = config.MagnetRadius;
						activeMagnet.ExcludeEntityIds = new HashSet<int>() { collectableEntityId };
					}
				}
				
				apply.CollectableEntityIds.Clear();

				applyPool.Del(appliedEntityId);
			}
		}
	}
}