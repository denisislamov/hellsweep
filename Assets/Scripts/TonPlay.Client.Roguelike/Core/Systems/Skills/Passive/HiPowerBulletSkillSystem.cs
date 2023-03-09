using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Skills;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Systems.Skills.Passive
{
	public class HiPowerBulletSkillSystem : IEcsInitSystem, IEcsRunSystem
	{
		private EcsWorld _world;
		private IHiPowerBulletSkillConfig _config;
		private ISharedData _sharedData;

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();

			_world = systems.GetWorld();
			_config = (IHiPowerBulletSkillConfig)_sharedData.SkillsConfigProvider.Get(SkillName.HiPowerBullet);
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
						.Inc<DamageMultiplierComponent>()
						.Exc<DeadComponent>()
						.Exc<HiPowerBulletSkill>()
						.End();

			var skillsPool = _world.GetPool<SkillsComponent>();
			var skillPool = _world.GetPool<HiPowerBulletSkill>();
			var damageMultiplierPool = _world.GetPool<DamageMultiplierComponent>();
			
			foreach (var entityId in filter)
			{
				ref var skills = ref skillsPool.Get(entityId);

				if (skills.Levels.ContainsKey(_config.SkillName) && skills.Levels[_config.SkillName] > 0)
				{
					ref var skill = ref skillPool.Add(entityId);
					ref var damageMultiplier = ref damageMultiplierPool.Get(entityId);
					
					skill.Level = skills.Levels[_config.SkillName];
					UpgradeDamageMultiplier(ref damageMultiplier, skill);
				}
			}

			filter = _world
					.Filter<SkillsComponent>()
					.Inc<HiPowerBulletSkill>()
					.Inc<DamageMultiplierComponent>()
					.Exc<DeadComponent>()
					.End();

			foreach (var entityId in filter)
			{
				ref var skills = ref skillsPool.Get(entityId);
				ref var skill = ref skillPool.Get(entityId);
				ref var damageMultiplier = ref damageMultiplierPool.Get(entityId);

				if (skills.Levels.ContainsKey(_config.SkillName) && skills.Levels[_config.SkillName] != skill.Level)
				{
					skill.Level = skills.Levels[_config.SkillName];

					UpgradeDamageMultiplier(ref damageMultiplier, skill);
				}
			}
		}
		
		private void UpgradeDamageMultiplier(ref DamageMultiplierComponent damageMultiplier, HiPowerBulletSkill skill)
		{
			damageMultiplier.Value *= _config.GetLevelConfig(skill.Level).MultiplierValue;
		}
	}
}