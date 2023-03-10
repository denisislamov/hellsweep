using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Skills;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Systems.Skills.Passive
{
	public class HiPowerMagnetSkillSystem : IEcsInitSystem, IEcsRunSystem
	{
		private EcsWorld _world;
		private IHiPowerMagnetSkillConfig _config;
		private ISharedData _sharedData;

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();

			_world = systems.GetWorld();
			_config = (IHiPowerMagnetSkillConfig)_sharedData.SkillsConfigProvider.Get(SkillName.HIPowerMagnet);
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
						.Inc<CollisionAreaWithCollectablesComponent>()
						.Exc<DeadComponent>()
						.Exc<HiPowerMagnetSkill>()
						.End();

			var skillsPool = _world.GetPool<SkillsComponent>();
			var skillPool = _world.GetPool<HiPowerMagnetSkill>();
			var damageMultiplierPool = _world.GetPool<CollisionAreaWithCollectablesComponent>();
			
			foreach (var entityId in filter)
			{
				ref var skills = ref skillsPool.Get(entityId);

				if (skills.Levels.ContainsKey(_config.SkillName) && skills.Levels[_config.SkillName] > 0)
				{
					ref var skill = ref skillPool.Add(entityId);
					ref var damageMultiplier = ref damageMultiplierPool.Get(entityId);
					
					skill.Level = skills.Levels[_config.SkillName];
					UpgradeCollisionWithCollectablesScale(ref damageMultiplier, skill);
				}
			}

			filter = _world
					.Filter<SkillsComponent>()
					.Inc<HiPowerMagnetSkill>()
					.Inc<CollisionAreaWithCollectablesComponent>()
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

					UpgradeCollisionWithCollectablesScale(ref damageMultiplier, skill);
				}
			}
		}
		
		private void UpgradeCollisionWithCollectablesScale(ref CollisionAreaWithCollectablesComponent collision, HiPowerMagnetSkill skill)
		{
			if (skill.Level > 1)
			{
				collision.CollisionArea.Scale /= _config.GetLevelConfig(skill.Level - 1).MultiplierValue;
			}
			
			collision.CollisionArea.Scale *= _config.GetLevelConfig(skill.Level).MultiplierValue;
		}
	}
}