using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ApplyBombCollectableSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world
						.Filter<ApplyBombCollectableComponent>()
						.Exc<DeadComponent>()
						.End();
			var bombPool = world.GetPool<BombCollectableComponent>();
			var applyPool = world.GetPool<ApplyBombCollectableComponent>();
			var preparePool = world.GetPool<PrepareToExplodeCollectedBombsComponent>();
			var destroyPool = world.GetPool<DestroyComponent>();

			foreach (var entityId in filter)
			{
				ref var apply = ref applyPool.Get(entityId);

				if (apply.CollectableEntityIds.Count <= 0)
				{
					continue;
				}

				if (!preparePool.Has(entityId))
				{
					preparePool.Add(entityId);
				}

				ref var prepare = ref preparePool.Get(entityId);

				prepare.Bombs ??= new List<(float, IBombCollectableConfig)>();

				foreach (var collectableEntityId in apply.CollectableEntityIds)
				{
					ref var bomb = ref bombPool.Get(collectableEntityId);

					prepare.Bombs.Add((bomb.Config.TimeToExplode, bomb.Config));

					destroyPool.Add(collectableEntityId);
				}

				apply.CollectableEntityIds.Clear();
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}