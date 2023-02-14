using System;
using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Skills;
using TonPlay.Client.Roguelike.Core.Effects;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TonPlay.Client.Roguelike.Core.Systems.Skills
{
	public class ForcefieldDeviceSkillSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly IOverlapExecutor _overlapExecutor;
		private List<int> _overlappedEntities = new List<int>();
		private readonly KDQuery _query = new KDQuery();

		private EcsWorld _world;
		private IForcefieldDeviceSkillConfig _config;
		private ICompositeViewPool _pool;
		private IViewPoolIdentity _poolIdentity;
		private ISharedData _sharedData;

		public ForcefieldDeviceSkillSystem(IOverlapExecutor overlapExecutor)
		{
			_overlapExecutor = overlapExecutor;
		}

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();

			_world = systems.GetWorld();
			_config = (IForcefieldDeviceSkillConfig) _sharedData.SkillsConfigProvider.Get(SkillName.ForcefieldDevice);
		}

		public void Run(EcsSystems systems)
		{
			AddSkillComponentIfDoesntExist();
			ApplyDamageToCollidedEntities();
			SyncEffectSize();
		}
		
		private void ApplyDamageToCollidedEntities()
		{
			var filter = _world
						.Filter<SkillsComponent>()
						.Inc<ForcefieldDeviceSkill>()
						.Inc<PositionComponent>()
						.Exc<DeadComponent>()
						.End();

			var skillsPool = _world.GetPool<SkillsComponent>();
			var positionPool = _world.GetPool<PositionComponent>();
			var applyDamagePool = _world.GetPool<ApplyDamageComponent>();

			foreach (var entityId in filter)
			{
				ref var position = ref positionPool.Get(entityId);
				ref var skills = ref skillsPool.Get(entityId);

				var level = skills.Levels[SkillName.ForcefieldDevice];
				var levelConfig = _config.GetLevelConfig(level);

				var count = _overlapExecutor.Overlap(_query, position.Position, levelConfig.CollisionAreaConfig, ref _overlappedEntities, levelConfig.CollisionLayerMask);
				for (var i = 0; i < count; i++)
				{
					var overlappedEntityId = _overlappedEntities[i];

					ref var applyDamage = ref applyDamagePool.AddOrGet(overlappedEntityId);
					applyDamage.Damage += levelConfig.Damage;
				}
				
				_overlappedEntities.Clear();
			}
		}
		
		private void SyncEffectSize()
		{
			var filter = _world
						.Filter<SkillsComponent>()
						.Inc<ForcefieldDeviceSkill>()
						.Inc<PositionComponent>()
						.Exc<DeadComponent>()
						.End();

			var skillPool = _world.GetPool<ForcefieldDeviceSkill>();
			var skillsPool = _world.GetPool<SkillsComponent>();
			var transformPool = _world.GetPool<TransformComponent>();

			foreach (var entityId in filter)
			{
				ref var skill = ref skillPool.Get(entityId);
				ref var skills = ref skillsPool.Get(entityId);
				ref var effectTransform = ref transformPool.Get(skill.EffectEntityId);

				var level = skills.Levels[SkillName.ForcefieldDevice];
				var levelConfig = _config.GetLevelConfig(level);

				var requiredSqrMagnitude = levelConfig.Size * levelConfig.Size;
				var currentSqrMagnitude = effectTransform.Transform.localScale.sqrMagnitude;

				if (Math.Abs(requiredSqrMagnitude - currentSqrMagnitude) > 0.0001f)
				{
					effectTransform.Transform.localScale = Vector3.one * levelConfig.Size * 2f;
				}
			}
		}

		private void AddSkillComponentIfDoesntExist()
		{
			var filter = _world
						.Filter<SkillsComponent>()
						.Inc<PositionComponent>()
						.Exc<DeadComponent>()
						.Exc<ForcefieldDeviceSkill>()
						.End();

			var positionPool = _world.GetPool<PositionComponent>();
			var skillsPool = _world.GetPool<SkillsComponent>();
			var skillPool = _world.GetPool<ForcefieldDeviceSkill>();

			foreach (var entityId in filter)
			{
				ref var skills = ref skillsPool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);

				if (skills.Levels.ContainsKey(_config.SkillName) && skills.Levels[_config.SkillName] > 0)
				{
					ref var skill = ref skillPool.Add(entityId);
					var level = skills.Levels[_config.SkillName];
					var levelConfig = _config.GetLevelConfig(level);
					var effectEntity = CreateEffect(position.Position, entityId, levelConfig.Size);
					skill.EffectEntityId = effectEntity.Id;
				}
			}
		}
		
		private EcsEntity CreateEffect(Vector2 position, int parentEntityId, float size)
		{
			var entity = _world.NewEntity();
			var effect = Object.Instantiate(_config.EffectView);
			var transform = effect.transform;
			transform.position = position;
			transform.localScale = Vector3.one * size;

			entity.AddPositionComponent(position);
			entity.AddTransformComponent(effect.transform);
			entity.AddForcefieldDeviceEffectComponent(parentEntityId);
			entity.AddSyncPositionWithAnotherEntityComponent(parentEntityId);

			return entity;
		}
	}
}