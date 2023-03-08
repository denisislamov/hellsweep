using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Skills;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems.Skills
{
	public class EnergyDrinkSkillSystem : IEcsInitSystem, IEcsRunSystem
	{
		private EcsWorld _world;
		private IEnergyDrinkSkillConfig _config;
		private ISharedData _sharedData;

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();

			_world = systems.GetWorld();
			_config = (IEnergyDrinkSkillConfig)_sharedData.SkillsConfigProvider.Get(SkillName.EnergyDrink);
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			AddSkillComponentIfDoesntExist();
			RestoreHealth();
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
		
		private void RestoreHealth()
		{
			var filter = _world
						.Filter<SkillsComponent>()
						.Inc<HealthComponent>()
						.Inc<EnergyDrinkSkill>()
						.Exc<DeadComponent>()
						.End();
			
			var skillsPool = _world.GetPool<SkillsComponent>();
			var healthPool = _world.GetPool<HealthComponent>();
			var changeHealthPool = _world.GetPool<ChangeHealthEvent>();

			foreach (var entityId in filter)
			{
				ref var skills = ref skillsPool.Get(entityId);
				ref var health = ref healthPool.Get(entityId);
				ref var changeHealth = ref changeHealthPool.AddOrGet(entityId);

				var multiplier = _config.GetLevelConfig(skills.Levels[_config.SkillName]).IncreaseHealthMultiplier;
				changeHealth.DifferenceValue += health.MaxHealth * multiplier * Time.deltaTime;
			}
		}

		private void AddSkillComponentIfDoesntExist()
		{
			var filter = _world
						.Filter<SkillsComponent>()
						.Inc<HealthComponent>()
						.Exc<DeadComponent>()
						.Exc<EnergyDrinkSkill>()
						.End();

			var skillsPool = _world.GetPool<SkillsComponent>();
			var skillPool = _world.GetPool<EnergyDrinkSkill>();

			foreach (var entityId in filter)
			{
				ref var skills = ref skillsPool.Get(entityId);

				if (skills.Levels.ContainsKey(_config.SkillName) && skills.Levels[_config.SkillName] > 0)
				{
					skillPool.Add(entityId);
				}
			}
		}
	}
}