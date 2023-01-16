using System;
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
			var filter = world.Filter<PlayerComponent>()
							  .Inc<HealthComponent>()
							  .Inc<ExperienceComponent>()
							  .Exc<DeadComponent>().End();
			var healthComponents = world.GetPool<HealthComponent>();
			var experienceComponents = world.GetPool<ExperienceComponent>();

			var playerModel = systems.GetShared<SharedData>()?.GameModel?.PlayerModel;

			if (playerModel is null) return;
			
			var playerData = playerModel.ToData();

			foreach (var entityId in filter)
			{
				ref var healthComponent = ref healthComponents.Get(entityId);
				ref var experienceComponent = ref experienceComponents.Get(entityId);

				if (Math.Abs(playerData.Health - healthComponent.CurrentHealth) > 0.001f)
				{
					playerData.Health = healthComponent.CurrentHealth;
				}

				if (Math.Abs(playerData.MaxHealth - healthComponent.MaxHealth) > 0.001f)
				{
					playerData.MaxHealth = healthComponent.MaxHealth;
				}

				if (Math.Abs(playerData.Experience - experienceComponent.Value) > 0.001f)
				{
					playerData.Experience = experienceComponent.Value;
				}

				if (Math.Abs(playerData.MaxExperience - experienceComponent.MaxValue) > 0.001f)
				{
					playerData.MaxExperience = experienceComponent.MaxValue;
				}

				if (playerData.SkillsData.Level != experienceComponent.Level)
				{
					playerData.SkillsData.Level = experienceComponent.Level;
				}
			}
			
			playerModel.Update(playerData);
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}