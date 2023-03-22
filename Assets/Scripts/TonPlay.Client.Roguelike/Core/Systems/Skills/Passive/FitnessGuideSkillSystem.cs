using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Skills;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Systems.Skills.Passive
{
	public class FitnessGuideSkillSystem : IEcsInitSystem, IEcsRunSystem
	{
		private EcsWorld _world;
		private IFitnessGuideSkillConfig _config;
		private ISharedData _sharedData;

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();

			_world = systems.GetWorld();
			_config = (IFitnessGuideSkillConfig)_sharedData.SkillsConfigProvider.Get(SkillName.FitnessGuide);
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			SyncSkillComponent();
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}

		private void SyncSkillComponent()
		{
			var filter = _world
						.Filter<SkillsComponent>()
						.Inc<HealthComponent>()
						.Exc<DeadComponent>()
						.Exc<FitnessGuideSkill>()
						.End();

			var skillsPool = _world.GetPool<SkillsComponent>();
			var skillPool = _world.GetPool<FitnessGuideSkill>();
			var healthPool = _world.GetPool<HealthComponent>();

			foreach (var entityId in filter)
			{
				ref var skills = ref skillsPool.Get(entityId);

				if (skills.Levels.ContainsKey(_config.SkillName) && skills.Levels[_config.SkillName] > 0)
				{
					ref var skill = ref skillPool.Add(entityId);
					ref var health = ref healthPool.Get(entityId);
					
					skill.Level = skills.Levels[_config.SkillName];
					
					UpgradeHealth(ref health, skill);
				}
			}

			filter = _world
					.Filter<SkillsComponent>()
					.Inc<FitnessGuideSkill>()
					.Inc<HealthComponent>()
					.Exc<DeadComponent>()
					.End();

			foreach (var entityId in filter)
			{
				ref var skills = ref skillsPool.Get(entityId);
				ref var skill = ref skillPool.Get(entityId);
				ref var health = ref healthPool.Get(entityId);

				if (skills.Levels.ContainsKey(_config.SkillName) && skills.Levels[_config.SkillName] != skill.Level)
				{
					skill.Level = skills.Levels[_config.SkillName];

					UpgradeHealth(ref health, skill);
				}
			}
		}
		
		private void UpgradeHealth(ref HealthComponent health, FitnessGuideSkill skill)
		{
			var previousMaxHealth = health.MaxHealth;
			var upgradedMaxHealth = health.InitialMaxHealth + health.InitialMaxHealth * _config.GetLevelConfig(skill.Level).MultiplierValue;
			var diff = upgradedMaxHealth - previousMaxHealth;
			health.MaxHealth = upgradedMaxHealth;
			health.CurrentHealth += diff;
		}
	}
}