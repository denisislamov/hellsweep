using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Client.Roguelike.Core.Collision
{
	public class PlayerWithUtilityCollisionProcessor : ICollisionProcessor
	{
		private readonly EcsWorld _ecsWorld;

		public PlayerWithUtilityCollisionProcessor(EcsWorld ecsWorld)
		{
			_ecsWorld = ecsWorld;
		}

		public void Process(ref int utilityEntityId)
		{
			TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
			var players = _ecsWorld.Filter<PlayerComponent>().Inc<HealthComponent>().End();

			var playerEntityId = EcsEntity.DEFAULT_ID;
			foreach (var entityId in players)
			{
				playerEntityId = entityId;
			}

			if (playerEntityId == EcsEntity.DEFAULT_ID || !_ecsWorld.IsEntityAlive(playerEntityId))
			{
				return;
			}

			AddCollidedWithUtilityComponentToPlayer(utilityEntityId, playerEntityId);
			TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}

		private void AddCollidedWithUtilityComponentToPlayer(
			int utilityEntityId,
			int playerEntityId)
		{
			var collectablePool = _ecsWorld.GetPool<CollectableComponent>();
			var usedPool = _ecsWorld.GetPool<UsedComponent>();

			if (collectablePool.Has(utilityEntityId) && !usedPool.Has(utilityEntityId))
			{
				usedPool.Add(utilityEntityId);

				ref var applyCollectable = ref _ecsWorld.NewEntity().Add<ApplyCollectableComponent>();
				applyCollectable.AppliedEntityId = playerEntityId;
				applyCollectable.CollectableEntityId = utilityEntityId;
				applyCollectable.Started = false;
			}
		}
	}
}