using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class SyncRotationWithPositionDifferenceSystem : IEcsRunSystem, IEcsInitSystem
	{
		private EcsWorld[] _worlds;
		
		public void Init(EcsSystems systems)
		{
			_worlds = new EcsWorld[]
			{
				systems.GetWorld(),
				systems.GetWorld(RoguelikeConstants.Core.EFFECTS_WORLD_NAME)
			};
		}
		
		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			for (var index = 0; index < _worlds.Length; index++)
			{
				var world = _worlds[index];
				var filter = world.Filter<SyncRotationWithPositionDifferenceComponent>().Inc<RotationComponent>().Inc<PositionComponent>().End();
				var rotationPool = world.GetPool<RotationComponent>();
				var positionPool = world.GetPool<PositionComponent>();
				var syncRotationWithPositionPool = world.GetPool<SyncRotationWithPositionDifferenceComponent>();

				foreach (var entityId in filter)
				{
					ref var rotation = ref rotationPool.Get(entityId);
					ref var position = ref positionPool.Get(entityId);
					ref var syncRotationWithPosition = ref syncRotationWithPositionPool.Get(entityId);

					rotation.Direction = (position.Position - syncRotationWithPosition.LastPosition);
					rotation.Direction.Normalize();

					syncRotationWithPosition.LastPosition = Vector2.Lerp(syncRotationWithPosition.LastPosition, position.Position, Time.deltaTime*5);
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}