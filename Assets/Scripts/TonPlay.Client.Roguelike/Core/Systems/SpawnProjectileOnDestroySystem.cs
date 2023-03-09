using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Pooling.Identities;
using TonPlay.Client.Roguelike.Core.Weapons;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Core.Weapons.Configs.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.Views;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class SpawnProjectileOnDestroySystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var pool = systems.GetShared<ISharedData>().CompositeViewPool;
			var world = systems.GetWorld();
			var filter = world
						.Filter<DestroyComponent>()
						.Inc<ProjectileComponent>()
						.Inc<SpawnProjectileOnDestroyComponent>()
						.Inc<PositionComponent>()
						.End();

			var spawnPool = world.GetPool<SpawnProjectileOnDestroyComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var projectilePool = world.GetPool<ProjectileComponent>();
			var destroyOnTimerPool = world.GetPool<DestroyOnTimerComponent>();
			var damageOnCollisionPool = world.GetPool<DamageOnCollisionComponent>();
			var skillDurationMultiplierPool = world.GetPool<SkillDurationMultiplierComponent>();

			foreach (var entityId in filter)
			{
				ref var component = ref spawnPool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);
				ref var projectile = ref projectilePool.Get(entityId);

				var config = component.ProjectileConfig;
				var poolIdentity = new ProjectileConfigViewPoolIdentity(config);

				if (pool.TryGet<ProjectileView>(poolIdentity, out var poolObject))
				{
					var entity = ProjectileSpawner.SpawnProjectile(world, poolObject, config, position.Position, Random.onUnitSphere, component.CollisionLayerMask);

					if (component.ProjectileConfig.HasProperty<IInheritDamageOnCollisionProjectileConfigProperty>() &&
						damageOnCollisionPool.Has(entityId))
					{
						ref var parentDamage = ref damageOnCollisionPool.Get(entityId);
						ref var damage = ref damageOnCollisionPool.AddOrGet(entity.Id);
						damage.DamageProvider = parentDamage.DamageProvider;
					}

					if (component.ProjectileConfig.HasProperty<IDestroyOnTimerProjectileConfigProperty>() &&
						projectile.CreatorEntityId != EcsEntity.DEFAULT_ID &&
						skillDurationMultiplierPool.Has(projectile.CreatorEntityId))
					{
						ref var skillDurationMultiplier = ref skillDurationMultiplierPool.Get(projectile.CreatorEntityId);
						ref var destroyOnTimer = ref destroyOnTimerPool.AddOrGet(projectile.CreatorEntityId);
						destroyOnTimer.TimeLeft *= skillDurationMultiplier.Value;
					}
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}