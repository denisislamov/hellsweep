using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons;
using TonPlay.Client.Roguelike.Core.Weapons.Configs.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Pooling.Identities;
using TonPlay.Roguelike.Client.Core.Weapons.Configs;
using TonPlay.Roguelike.Client.Core.Weapons.Views;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class SpawnProjectileOnDestroySystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
 #region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var pool = systems.GetShared<ISharedData>().CompositeViewPool;
			var world = systems.GetWorld();
			var filter = world
						.Filter<DestroyComponent>()
						.Inc<SpawnProjectileOnDestroyComponent>()
						.Inc<PositionComponent>()
						.End();
			
			var spawnPool = world.GetPool<SpawnProjectileOnDestroyComponent>();
			var positionPool = world.GetPool<PositionComponent>();
			var damageOnCollisionPool = world.GetPool<DamageOnCollisionComponent>();

			foreach (var entityId in filter)
			{
				ref var component = ref spawnPool.Get(entityId);
				ref var position = ref positionPool.Get(entityId);

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
				}
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}