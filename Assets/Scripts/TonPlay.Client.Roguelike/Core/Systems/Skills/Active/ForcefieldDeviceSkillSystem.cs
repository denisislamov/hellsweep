using System;
using System.Collections.Generic;
using DataStructures.ViliWonka.KDTree;
using Leopotam.EcsLite;
using TonPlay.Client.Common.Extensions;
using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Skills;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Pooling.Interfaces;
using TonPlay.Client.Roguelike.Core.Skills;
using TonPlay.Client.Roguelike.Core.Skills.Config.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Core.Pooling.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TonPlay.Client.Roguelike.Core.Systems.Skills.Active
{
	public class ForcefieldDeviceSkillSystem : IEcsInitSystem, IEcsRunSystem
	{
		private readonly IOverlapExecutor _overlapExecutor;
		private readonly KDQuery _query = new KDQuery();
		private readonly KdTreeStorage _playerProjectilesKdTreeStorage;

		private readonly int _playerProjectilesLayer;
		private readonly int _enemyProjectilesLayer;

		private List<int> _overlappedEntities = new List<int>();

		private EcsWorld _world;
		private IForcefieldDeviceSkillConfig _config;
		private ICompositeViewPool _pool;
		private IViewPoolIdentity _poolIdentity;
		private ISharedData _sharedData;

		public ForcefieldDeviceSkillSystem(IOverlapExecutor overlapExecutor, KdTreeStorage playerProjectilesKdTreeStorage)
		{
			_overlapExecutor = overlapExecutor;
			_playerProjectilesKdTreeStorage = playerProjectilesKdTreeStorage;

			_playerProjectilesLayer = LayerMask.NameToLayer("PlayerProjectile");
			_enemyProjectilesLayer = LayerMask.NameToLayer("EnemyProjectile");
		}

		public void Init(EcsSystems systems)
		{
			_sharedData = systems.GetShared<ISharedData>();

			_world = systems.GetWorld();
			_config = (IForcefieldDeviceSkillConfig)_sharedData.SkillsConfigProvider.Get(SkillName.ForcefieldDevice);
		}

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			AddSkillComponentIfDoesntExist();
			ApplyDamageToCollidedEntities();
			SyncEffectSize();
			SyncEffectLevelWithOwner();
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}

		private void ApplyDamageToCollidedEntities()
		{
			var filter = _world
						.Filter<SkillsComponent>()
						.Inc<ForcefieldDeviceSkill>()
						.Inc<PositionComponent>()
						.Inc<StackTryApplyDamageComponent>()
						.Inc<DamageMultiplierComponent>()
						.Exc<DeadComponent>()
						.End();

			var skillsPool = _world.GetPool<SkillsComponent>();
			var positionPool = _world.GetPool<PositionComponent>();
			var damageMultiplierPool = _world.GetPool<DamageMultiplierComponent>();
			var stackTryApplyDamagePool = _world.GetPool<StackTryApplyDamageComponent>();

			var overlapParams = OverlapParams.Create(_world);
			overlapParams.SetFilter(overlapParams.CreateDefaultFilterMask().End());
			overlapParams.Build();

			foreach (var entityId in filter)
			{
				ref var damageMultiplier = ref damageMultiplierPool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);
				ref var skills = ref skillsPool.Get(entityId);

				var level = skills.Levels[SkillName.ForcefieldDevice];
				var levelConfig = _config.GetLevelConfig(level);

				levelConfig.DamageProvider.DamageMultiplier = damageMultiplier.Value;

				//it's to prevent double damage applying for upgraded forcefield
				if (LayerMaskExt.ContainsLayer(levelConfig.CollisionLayerMask, _enemyProjectilesLayer))
				{
					continue;
				}

				var count = _overlapExecutor.Overlap(
					_query,
					position.Position,
					CollisionAreaFactory.Create(levelConfig.CollisionAreaConfig),
					ref _overlappedEntities,
					levelConfig.CollisionLayerMask,
					overlapParams);

				for (var i = 0; i < count; i++)
				{
					var overlappedEntityId = _overlappedEntities[i];

					ref var stack = ref stackTryApplyDamagePool.Get(entityId);
					stack.Stack.Push(new TryApplyDamageComponent()
					{
						DamageProvider = levelConfig.DamageProvider,
						VictimEntityId = overlappedEntityId,
					});
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

				var requiredSqrMagnitude = levelConfig.Size*levelConfig.Size;
				var currentSqrMagnitude = effectTransform.Transform.localScale.sqrMagnitude;

				if (Math.Abs(requiredSqrMagnitude - currentSqrMagnitude) > 0.0001f)
				{
					effectTransform.Transform.localScale = Vector3.one*(levelConfig.Size*2f);
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
			var level = 0;

			transform.position = position;
			transform.localScale = Vector3.one*size;

			entity.AddPositionComponent(position);
			entity.AddTransformComponent(effect.transform);
			entity.AddForcefieldDeviceEffectComponent(parentEntityId, level);
			entity.AddSyncPositionWithAnotherEntityComponent(parentEntityId);

			return entity;
		}

		private void SyncEffectLevelWithOwner()
		{
			var filter = _world.Filter<ForcefieldDeviceEffectComponent>().Inc<PositionComponent>().End();
			var skillsPool = _world.GetPool<SkillsComponent>();
			var forceFieldPool = _world.GetPool<ForcefieldDeviceEffectComponent>();
			var positionPool = _world.GetPool<PositionComponent>();

			foreach (var effectEntityIdx in filter)
			{
				ref var forceField = ref forceFieldPool.Get(effectEntityIdx);
				ref var position = ref positionPool.Get(effectEntityIdx);
				ref var skills = ref skillsPool.Get(forceField.ParentEntityId);

				if (skills.Levels[_config.SkillName] == forceField.Level)
				{
					continue;
				}

				forceField.Level = skills.Levels[_config.SkillName];

				var levelConfig = _config.GetLevelConfig(forceField.Level);

				//this code below is a real true bullshit, i know, i hope we will deal with it a lil bit later
				var entity = new EcsEntity(_world, effectEntityIdx);

				if (LayerMaskExt.ContainsLayer(levelConfig.CollisionLayerMask, _enemyProjectilesLayer))
				{
					if (!entity.Has<BlockApplyDamageTimerComponent>())
					{
						entity.AddBlockApplyDamageTimerComponent();
					}

					if (!entity.Has<StackTryApplyDamageComponent>())
					{
						entity.AddStackTryApplyDamageComponent();
					}

					if (!entity.Has<LayerComponent>())
					{
						entity.AddLayerComponent(_playerProjectilesLayer);
					}

					if (!entity.Has<CollisionComponent>())
					{
						entity.AddCollisionComponent(CollisionAreaFactory.Create(levelConfig.CollisionAreaConfig), levelConfig.CollisionLayerMask);
						entity.AddHasCollidedComponent();
						entity.AddDamageOnCollisionComponent(levelConfig.DamageProvider);
					}
					else
					{
						ref var collision = ref entity.Get<CollisionComponent>();
						collision.CollisionArea = CollisionAreaFactory.Create(levelConfig.CollisionAreaConfig);
						collision.LayerMask = levelConfig.CollisionLayerMask;

						ref var damageOnCollision = ref entity.Get<DamageOnCollisionComponent>();
						damageOnCollision.DamageProvider = levelConfig.DamageProvider;
					}

					if (!entity.Has<KdTreeElementComponent>())
					{
						var treeIndex = _playerProjectilesKdTreeStorage.AddEntity(entity.Id, position.Position);

						entity.AddKdTreeElementComponent(_playerProjectilesKdTreeStorage, treeIndex);
						entity.AddDrawDebugKdTreePositionComponent();
					}
				}
				else
				{
					if (entity.Has<BlockApplyDamageTimerComponent>())
					{
						entity.Del<BlockApplyDamageTimerComponent>();
					}

					if (entity.Has<StackTryApplyDamageComponent>())
					{
						entity.Del<StackTryApplyDamageComponent>();
					}

					if (entity.Has<LayerComponent>())
					{
						entity.Del<LayerComponent>();
					}

					if (entity.Has<KdTreeElementComponent>())
					{
						_playerProjectilesKdTreeStorage.RemoveEntity(entity.Id);

						entity.Del<KdTreeElementComponent>();
					}

					if (entity.Has<CollisionComponent>())
					{
						entity.Del<CollisionComponent>();
						entity.Del<HasCollidedComponent>();
						entity.Del<DamageOnCollisionComponent>();
					}
				}
			}
		}
	}
}