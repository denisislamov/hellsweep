using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Signals;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine.Profiling;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class ShowAppliedDamageSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			Profiler.BeginSample(GetType().FullName);
#endregion
			var sharedData = systems.GetShared<ISharedData>();
			var world = systems.GetWorld();
			
			var filter = world
									 .Filter<ApplyDamageComponent>()
									 .Inc<PositionComponent>()
									 .Exc<DeadComponent>()
									 .Exc<PlayerComponent>()
									 .End();
			
			var applyDamageComponents = world.GetPool<ApplyDamageComponent>();
			var positionPool = world.GetPool<PositionComponent>();

			foreach (var entityId in filter)
			{
				ref var applyDamage = ref applyDamageComponents.Get(entityId);
				ref var position = ref positionPool.Get(entityId);
				
				//sharedData.SignalBus.Fire(new AppliedDamageSignal(applyDamage.Damage, position.Position));
				var entity = world.NewEntity();
				ref var spawnAppliedDamageIndicatorEvent = ref entity.Add<SpawnAppliedDamageIndicatorEvent>();
				ref var indicatorPosition = ref entity.Add<PositionComponent>();

				spawnAppliedDamageIndicatorEvent.Damage = applyDamage.Damage;
				indicatorPosition.Position = position.Position;
			}
#region Profiling End
			Profiler.EndSample();
#endregion 
		}
	}
}