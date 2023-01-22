using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class WeaponPositionSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var world = systems.GetWorld();

			var filter = world.Filter<WeaponComponent>().Inc<PositionComponent>().End();
			
			var weaponPool = world.GetPool<WeaponComponent>();
			var positionPool = world.GetPool<PositionComponent>();

			foreach (var entityId in filter)
			{
				ref var weapon = ref weaponPool.Get(entityId);
				
				if (positionPool.Has(weapon.OwnerEntityId))
				{
					ref var weaponPosition = ref positionPool.Get(entityId);
					ref var ownerPosition = ref positionPool.Get(weapon.OwnerEntityId);
					weaponPosition.Position = ownerPosition.Position;
				}
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}