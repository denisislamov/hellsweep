using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Pooling.Identities;
using TonPlay.Client.Roguelike.Core.UI;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class FadeColorAppliedDamageIndicatorSystem : IEcsRunSystem
	{
		private ISharedData _sharedData;
		private ICompositeViewPool _pool;
		private EcsWorld _world;
		private IViewPoolIdentity _poolIdentity;

		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var filter = world.Filter<AppliedDamageIndicator>().End();
			var indicatorPool = world.GetPool<AppliedDamageIndicator>();

			foreach (var entityId in filter)
			{
				ref var indicator = ref indicatorPool.Get(entityId);

				indicator.View.Color = Color.Lerp(indicator.View.Color, indicator.FadeOutColor, Time.deltaTime / indicator.FadeOutTime);
			}
		}
	}
}