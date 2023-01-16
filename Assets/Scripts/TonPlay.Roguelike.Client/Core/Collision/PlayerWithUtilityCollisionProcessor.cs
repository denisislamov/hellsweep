using System;
using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Collectables;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Roguelike.Client.Core.Collision
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
			var players = _ecsWorld.Filter<PlayerComponent>().Inc<HealthComponent>().End();

			var playerEntityId = EcsEntity.DEFAULT_ID;
			foreach (var entityId in players)
			{
				playerEntityId = entityId;
			}

			if (playerEntityId == EcsEntity.DEFAULT_ID)
			{
				return;
			}

			AddCollidedWithUtilityComponentToPlayer(utilityEntityId, playerEntityId);
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