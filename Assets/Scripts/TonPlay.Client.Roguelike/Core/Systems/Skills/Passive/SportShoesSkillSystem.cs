using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Skills;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Systems.Skills.Passive
{
	public class SportShoesSkillSystem : IEcsInitSystem, IEcsRunSystem
	{
		private EcsWorld _world;
		private ISportShoesSkillConfig _config;
		private ISharedData _sharedData;

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();

			_world = systems.GetWorld();
			_config = (ISportShoesSkillConfig)_sharedData.SkillsConfigProvider.Get(SkillName.SportShoes);
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
						.Inc<SpeedComponent>()
						.Exc<DeadComponent>()
						.Exc<SportShoesSkill>()
						.End();

			var skillsPool = _world.GetPool<SkillsComponent>();
			var skillPool = _world.GetPool<SportShoesSkill>();
			var speedPool = _world.GetPool<SpeedComponent>();

			foreach (var entityId in filter)
			{
				ref var skills = ref skillsPool.Get(entityId);

				if (skills.Levels.ContainsKey(_config.SkillName) && skills.Levels[_config.SkillName] > 0)
				{
					ref var skill = ref skillPool.Add(entityId);
					ref var speed = ref speedPool.Get(entityId);
					
					skill.Level = skills.Levels[_config.SkillName];
					
					UpgradeSpeed(ref speed, skill);
				}
			}

			filter = _world
					.Filter<SkillsComponent>()
					.Inc<SportShoesSkill>()
					.Inc<SpeedComponent>()
					.Exc<DeadComponent>()
					.End();

			foreach (var entityId in filter)
			{
				ref var skills = ref skillsPool.Get(entityId);
				ref var skill = ref skillPool.Get(entityId);
				ref var speed = ref speedPool.Get(entityId);

				if (skills.Levels.ContainsKey(_config.SkillName) && skills.Levels[_config.SkillName] != skill.Level)
				{
					skill.Level = skills.Levels[_config.SkillName];

					UpgradeSpeed(ref speed, skill);
				}
			}
		}
		
		private void UpgradeSpeed(ref SpeedComponent speed, SportShoesSkill skill)
		{
			speed.Map[MovementSpeedMultiplierType.SportShoes] = 1 + _config.GetLevelConfig(skill.Level).MultiplierValue;
		}
	}
}