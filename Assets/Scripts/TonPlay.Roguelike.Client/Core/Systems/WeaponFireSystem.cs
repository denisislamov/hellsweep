using System.Collections.Generic;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Interfaces;
using TonPlay.Roguelike.Client.Core.Weapons.FireStrategies;
using UnityEngine.Profiling;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class WeaponFireSystem : IEcsInitSystem, IEcsRunSystem
	{
		private IReadOnlyDictionary<WeaponFireType, IWeaponFireStrategy> _fireStrategies;

		public void Init(EcsSystems systems)
		{
			var world = systems.GetWorld();
			var sharedData = systems.GetShared<ISharedData>();
			
			_fireStrategies = new Dictionary<WeaponFireType, IWeaponFireStrategy>()
			{
				[WeaponFireType.Direct] = new DirectWeaponFireStrategy(world, sharedData)
			};
		}

		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();
			var filter = world.Filter<WeaponComponent>()
							  .Exc<WeaponFireBlockComponent>()
							  .Exc<DeadComponent>()
							  .End();

			var firePool = world.GetPool<WeaponComponent>();
			var blockPool = world.GetPool<WeaponFireBlockComponent>();
			var deadPool = world.GetPool<DeadComponent>();

			foreach (var weaponEntityId in filter)
			{
				ref var weapon = ref firePool.Get(weaponEntityId);

				if (deadPool.Has(weapon.OwnerEntityId))
				{
					deadPool.Add(weaponEntityId);
					continue;
				}

				_fireStrategies[weapon.FireType].Fire(ref weapon);

				AddFireBlockComponent(blockPool, weaponEntityId, weapon);
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}

		private static void AddFireBlockComponent(EcsPool<WeaponFireBlockComponent> blockPool, int entityId, WeaponComponent component)
		{
			ref var blockComponent = ref blockPool.Add(entityId);
			blockComponent.TimeLeft = component.FireDelay;
		}
	}
}