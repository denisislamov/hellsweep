using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collectables.Config.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class PrepareToExplodeCollectedBombsSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world
						.Filter<PrepareToExplodeCollectedBombsComponent>()
						.Inc<PositionComponent>()
						.End();
			var preparePool = world.GetPool<PrepareToExplodeCollectedBombsComponent>();
			var positionPool = world.GetPool<PositionComponent>();

			foreach (var entityId in filter)
			{
				ref var prepare = ref preparePool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);

				var explodedBombs = new List<(float, IBombCollectableConfig)>();

				for (var index = 0; index < prepare.Bombs.Count; index++)
				{
					var prepareBomb = prepare.Bombs[index];
					prepareBomb.Item1 -= Time.deltaTime;
					prepare.Bombs[index] = prepareBomb;

					if (prepare.Bombs[index].Item1 <= 0)
					{
						explodedBombs.Add(prepareBomb);

						var explosionEntity = world.NewEntity();
						explosionEntity.AddPositionComponent(position.Position);
						explosionEntity.AddExplosionComponent(
							prepareBomb.Item2.DamageProvider,
							prepareBomb.Item2.ExplodeCollisionAreaConfig,
							prepareBomb.Item2.LayerMask);
						explosionEntity.AddStackTryApplyDamageComponent();
						explosionEntity.AddBlockApplyDamageTimerComponent();
					}
				}

				foreach (var explodedBomb in explodedBombs)
				{
					prepare.Bombs.Remove(explodedBomb);
				}

				explodedBombs.Clear();

				if (prepare.Bombs.Count == 0)
				{
					preparePool.Del(entityId);
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}