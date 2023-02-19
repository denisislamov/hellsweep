using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using UniRx;
using UnityEngine.Profiling;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class TryApplyDamageSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			Profiler.BeginSample(GetType().FullName);
#endregion
			var sharedData = systems.GetShared<ISharedData>();
			var world = systems.GetWorld();
			
			var filter = world
						.Filter<StackTryApplyDamageComponent>()
						.Inc<BlockApplyDamageTimerComponent>()
						.Exc<DeadComponent>()
						.End();
			
			var stackPool = world.GetPool<StackTryApplyDamageComponent>();
			var applyPool = world.GetPool<ApplyDamageComponent>();
			var blockPool = world.GetPool<BlockApplyDamageTimerComponent>();

			foreach (var entityId in filter)
			{
				ref var stack = ref stackPool.Get(entityId);
				ref var block = ref blockPool.Get(entityId);

				while (stack.Stack.Count > 0)
				{
					var tryApply = stack.Stack.Pop();

					if (block.Blocked.ContainsKey(tryApply.DamageProvider.DamageSource) &&
						block.Blocked[tryApply.DamageProvider.DamageSource].ContainsKey(tryApply.VictimEntityId) &&
						block.Blocked[tryApply.DamageProvider.DamageSource][tryApply.VictimEntityId].Value > 0)
					{
						continue;
					}

					ref var applyDamage = ref applyPool.AddOrGet(tryApply.VictimEntityId);
					applyDamage.Damage += tryApply.DamageProvider.Damage;

					if (tryApply.DamageProvider.Rate < 0.000001f)
					{
						continue;
					}
					
					if (!block.Blocked.ContainsKey(tryApply.DamageProvider.DamageSource))
					{
						block.Blocked.Add(tryApply.DamageProvider.DamageSource, new Dictionary<int, ReactiveProperty<float>>());
					}

					if (!block.Blocked[tryApply.DamageProvider.DamageSource].ContainsKey(tryApply.VictimEntityId))
					{
						block.Blocked[tryApply.DamageProvider.DamageSource].Add(tryApply.VictimEntityId, new ReactiveProperty<float>(0f));
					}

					block.Blocked[tryApply.DamageProvider.DamageSource][tryApply.VictimEntityId].SetValueAndForceNotify(tryApply.DamageProvider.Rate);
				}
			}
#region Profiling End
			Profiler.EndSample();
#endregion 
		}
	}
}