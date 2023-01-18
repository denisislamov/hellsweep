using System;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Systems
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
							  .Inc<SkillsComponent>()
							  .Exc<DeadComponent>()
							  .End();
			var healthPool = world.GetPool<HealthComponent>();
			var expPool = world.GetPool<ExperienceComponent>();
			var skillsPool = world.GetPool<SkillsComponent>();

			var playerModel = systems.GetShared<SharedData>()?.GameModel?.PlayerModel;

			if (playerModel is null) return;
			
			var playerData = playerModel.ToData();

			foreach (var entityId in filter)
			{
				ref var health = ref healthPool.Get(entityId);
				ref var exp = ref expPool.Get(entityId);
				ref var skills = ref skillsPool.Get(entityId);

				if (Math.Abs(playerData.Health - health.CurrentHealth) > 0.001f)
				{
					playerData.Health = health.CurrentHealth;
				}

				if (Math.Abs(playerData.MaxHealth - health.MaxHealth) > 0.001f)
				{
					playerData.MaxHealth = health.MaxHealth;
				}

				if (Math.Abs(playerData.Experience - exp.Value) > 0.001f)
				{
					playerData.Experience = exp.Value;
				}

				if (Math.Abs(playerData.MaxExperience - exp.MaxValue) > 0.001f)
				{
					playerData.MaxExperience = exp.MaxValue;
				}

				if (playerData.SkillsData.Level != exp.Level)
				{
					playerData.SkillsData.Level = exp.Level;
				}

				foreach (var kvp in skills.Levels)
				{
					var skillName = kvp.Key;
					var value = kvp.Value;

					if (!playerData.SkillsData.SkillLevels.ContainsKey(skillName))
					{
						playerData.SkillsData.SkillLevels.Add(skillName, value);
					} 
						
					playerData.SkillsData.SkillLevels[skillName] = value;
				}
			}
			
			playerModel.Update(playerData);
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}