using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Skills;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Systems.Skills.Passive
{
	public class RoninOyoroiSkillSystem : IEcsInitSystem, IEcsRunSystem
	{
		private EcsWorld _world;
		private IRoninOyoroiSkillConfig _config;
		private ISharedData _sharedData;

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();

			_world = systems.GetWorld();
			_config = (IRoninOyoroiSkillConfig) _sharedData.SkillsConfigProvider.Get(SkillName.RoninOyoroi);
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			SyncSkillComponent();
			ReduceDamage();
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
		
		private void ReduceDamage()
		{
			var filter = _world
						.Filter<SkillsComponent>()
						.Inc<RoninOyoroiSkill>()
						.Inc<ApplyDamageComponent>()
						.Exc<DeadComponent>()
						.End();

			var applyDamagePool = _world.GetPool<ApplyDamageComponent>();
			var skillPool = _world.GetPool<RoninOyoroiSkill>();

			foreach (var entityId in filter)
			{
				ref var applyDamage = ref applyDamagePool.Get(entityId);
				ref var skill = ref skillPool.Get(entityId);

				applyDamage.Damage -= applyDamage.Damage * skill.ReduceMultiplierValue;
			}
		}

		private void SyncSkillComponent()
		{
			var filter = _world
						.Filter<SkillsComponent>()
						.Exc<DeadComponent>()
						.End();

			var skillsPool = _world.GetPool<SkillsComponent>();
			var skillPool = _world.GetPool<RoninOyoroiSkill>();

			foreach (var entityId in filter)
			{
				ref var skills = ref skillsPool.Get(entityId);
				ref var skill = ref skillPool.AddOrGet(entityId);

				if (skills.Levels.ContainsKey(_config.SkillName) && skills.Levels[_config.SkillName] != skill.Level)
				{
					skill.Level = skills.Levels[_config.SkillName];
					skill.ReduceMultiplierValue = _config.GetLevelConfig(skill.Level).MultiplierValue;
				}
			}
		}
	}
}