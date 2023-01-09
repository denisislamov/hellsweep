using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class UpdatePlayerModelSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world.Filter<PlayerComponent>().Inc<HealthComponent>().Exc<DeadComponent>().End();
			var healthComponents = world.GetPool<HealthComponent>();

			var playerModel = systems.GetShared<SharedData>()?.GameModel?.PlayerModel;

			if (playerModel is null) return;
			
			var playerData = playerModel.ToData();

			foreach (var entityId in filter)
			{
				ref var healthComponent = ref healthComponents.Get(entityId);

				if (playerData.Health != healthComponent.CurrentHealth)
					playerData.Health = healthComponent.CurrentHealth;
				
				if (playerData.MaxHealth != healthComponent.MaxHealth)
					playerData.MaxHealth = healthComponent.MaxHealth;
			}
			
			playerModel.Update(playerData);
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}