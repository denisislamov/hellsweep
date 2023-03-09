using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Skills;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Systems.Skills.Passive
{
	public class ExoBracerSkillSystem : IEcsInitSystem, IEcsRunSystem
	{
		private EcsWorld _world;
		private IExoBracerSkillConfig _config;
		private ISharedData _sharedData;

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();

			_world = systems.GetWorld();
			_config = (IExoBracerSkillConfig)_sharedData.SkillsConfigProvider.Get(SkillName.ExoBracer);
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
						.Inc<SkillDurationMultiplierComponent>()
						.Exc<DeadComponent>()
						.Exc<ExoBracerSkill>()
						.End();

			var durationPool = _world.GetPool<SkillDurationMultiplierComponent>();
			var skillsPool = _world.GetPool<SkillsComponent>();
			var skillPool = _world.GetPool<ExoBracerSkill>();
			
			foreach (var entityId in filter)
			{
				ref var duration = ref durationPool.Get(entityId);
				ref var skills = ref skillsPool.Get(entityId);

				if (skills.Levels.ContainsKey(_config.SkillName) && skills.Levels[_config.SkillName] > 0)
				{
					ref var skill = ref skillPool.Add(entityId);
					
					skill.Level = skills.Levels[_config.SkillName];
					
					UpdateSkillDurationMultiplier(ref duration, skill);
				}
			}

			filter = _world
					.Filter<SkillsComponent>()
					.Inc<ExoBracerSkill>()
					.Inc<SkillDurationMultiplierComponent>()
					.Exc<DeadComponent>()
					.End();

			foreach (var entityId in filter)
			{
				ref var duration = ref durationPool.Get(entityId);
				ref var skills = ref skillsPool.Get(entityId);
				ref var skill = ref skillPool.Get(entityId);

				if (skills.Levels.ContainsKey(_config.SkillName) && skills.Levels[_config.SkillName] != skill.Level)
				{
					skill.Level = skills.Levels[_config.SkillName];
					
					UpdateSkillDurationMultiplier(ref duration, skill);
				}
			}
		}
		
		private void UpdateSkillDurationMultiplier(ref SkillDurationMultiplierComponent damageMultiplier, ExoBracerSkill skill)
		{
			damageMultiplier.Value *= _config.GetLevelConfig(skill.Level).MultiplierValue;
		}
	}
}