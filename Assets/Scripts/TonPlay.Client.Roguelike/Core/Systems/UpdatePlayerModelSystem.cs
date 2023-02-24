using System;
using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class UpdatePlayerModelSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();
			var filter = world.Filter<PlayerComponent>()
							  .Inc<HealthComponent>()
							  .Inc<ExperienceComponent>()
							  .Inc<SkillsComponent>()
							  .Inc<GoldComponent>()
							  .Inc<ProfileExperienceComponent>()
							  .Exc<DeadComponent>()
							  .End();
			var healthPool = world.GetPool<HealthComponent>();
			var expPool = world.GetPool<ExperienceComponent>();
			var goldPool = world.GetPool<GoldComponent>();
			var skillsPool = world.GetPool<SkillsComponent>();
			var profileExpPool = world.GetPool<ProfileExperienceComponent>();

			var playerModel = systems.GetShared<SharedData>()?.GameModel?.PlayerModel;

			if (playerModel is null) return;

			var playerData = playerModel.ToData();

			foreach (var entityId in filter)
			{
				ref var health = ref healthPool.Get(entityId);
				ref var exp = ref expPool.Get(entityId);
				ref var gold = ref goldPool.Get(entityId);
				ref var skills = ref skillsPool.Get(entityId);
				ref var profileExp = ref profileExpPool.Get(entityId);

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

				if (playerData.MatchProfileGainModel.Gold != Mathf.RoundToInt(gold.Value))
				{
					playerData.MatchProfileGainModel.Gold = Mathf.RoundToInt(gold.Value);
				}

				if (Math.Abs(playerData.MatchProfileGainModel.ProfileExperience - profileExp.Value) > 0.0001f)
				{
					playerData.MatchProfileGainModel.ProfileExperience = profileExp.Value;
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
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}