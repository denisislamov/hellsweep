using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Utilities;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class TransformRotationSystem : IEcsRunSystem, IEcsInitSystem
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
				var filter = world.Filter<RotationComponent>()
								  .Inc<TransformComponent>()
								  .Exc<IgnoreTransformRotation>()
								  .Exc<RigidbodyComponent>()
								  .Exc<DeadComponent>()
								  .Exc<InactiveComponent>()
								  .End();
				var rotationComponents = world.GetPool<RotationComponent>();
				var transformComponents = world.GetPool<TransformComponent>();

				foreach (var entityId in filter)
				{
					ref var rotation = ref rotationComponents.Get(entityId);
					ref var transform = ref transformComponents.Get(entityId);

					transform.Transform.right = rotation.Direction;
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}