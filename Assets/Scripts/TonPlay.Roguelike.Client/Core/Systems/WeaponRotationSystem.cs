using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class WeaponRotationSystem : IEcsRunSystem
	{
		public void Run(EcsSystems systems)
		{
			var world = systems.GetWorld();

			var filter = world.Filter<WeaponComponent>().Inc<TransformComponent>().End();
			
			var weaponPool = world.GetPool<WeaponComponent>();
			var rotationPool = world.GetPool<RotationComponent>();
			var transformPool = world.GetPool<TransformComponent>();

			foreach (var entityId in filter)
			{
				ref var weapon = ref weaponPool.Get(entityId);
				ref var transform = ref transformPool.Get(entityId);
				
				if (rotationPool.Has(weapon.OwnerEntityId))
				{
					ref var rotation = ref rotationPool.Get(weapon.OwnerEntityId);
					transform.Transform.right = rotation.Direction;
				}
			}
		}
	}
}