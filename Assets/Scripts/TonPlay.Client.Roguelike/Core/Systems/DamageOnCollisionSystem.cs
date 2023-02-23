using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine.Profiling;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class DamageOnCollisionSystem : IEcsRunSystem
	{
		private readonly IOverlapExecutor _overlapExecutor;

		public void Run(EcsSystems systems)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var world = systems.GetWorld();

			var filter = world.Filter<DamageOnCollisionComponent>()
							  .Inc<HasCollidedComponent>()
							  .Inc<StackTryApplyDamageComponent>()
							  .Exc<InactiveComponent>()
							  .Exc<BlockDamageOnCollisionComponent>()
							  .End();

			var damagePool = world.GetPool<DamageOnCollisionComponent>();
			var stackTryApplyDamagePool = world.GetPool<StackTryApplyDamageComponent>();
			var hasCollidedPool = world.GetPool<HasCollidedComponent>();

			foreach (var entityId in filter)
			{
				ref var hasCollided = ref hasCollidedPool.Get(entityId);
				ref var damage = ref damagePool.Get(entityId);

				var count = hasCollided.CollidedEntityIds.Count;

				for (int i = 0; i < count; i++)
				{
					var overlappedEntityId = hasCollided.CollidedEntityIds[i];

					ref var stack = ref stackTryApplyDamagePool.Get(entityId);
					stack.Stack.Push(new TryApplyDamageComponent()
					{
						DamageProvider = damage.DamageProvider,
						VictimEntityId = overlappedEntityId,
					});
				}
			}
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}
	}
}