using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.CollisionProcessors.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.CollisionProcessors
{
	public class EnemyToPlayerCollisionProcessor : ICollisionProcessor
	{
		private readonly EcsWorld _ecsWorld;
		private readonly ISharedData _sharedData;

		public EnemyToPlayerCollisionProcessor(EcsWorld ecsWorld, ISharedData sharedData)
		{
			_ecsWorld = ecsWorld;
			_sharedData = sharedData;
		}
		
		public void Process(ref Collider2D collider)
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

			var enemyEntityId = _sharedData.ColliderToEntityMap[collider];
				
			AddApplyDamageComponentToPlayer(enemyEntityId.Id, playerEntityId);
		}
		
		private void AddApplyDamageComponentToPlayer(
			int enemyEntityId, 
			int playerEntityId)
		{
			var damageDataComponents = _ecsWorld.GetPool<DamageOnCollisionComponent>();
			var applyDamageComponents = _ecsWorld.GetPool<ApplyDamageComponent>();
			
			ref var damageComponent = ref damageDataComponents.Get(enemyEntityId);
			if (applyDamageComponents.Has(playerEntityId))
			{
				ref var playerApplyDamageComponent = ref applyDamageComponents.Get(playerEntityId);
				playerApplyDamageComponent.Damage += damageComponent.Damage;
			}
			else
			{
				ref var playerApplyDamageComponent = ref applyDamageComponents.Add(playerEntityId);
				playerApplyDamageComponent.Damage = damageComponent.Damage;
			}
		}
	}
}