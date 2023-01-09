using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;
using TonPlay.Roguelike.Client.Core.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Collision
{
	public class PlayerWithEnemyCollisionProcessor : ICollisionProcessor
	{
		private readonly EcsWorld _ecsWorld;

		public PlayerWithEnemyCollisionProcessor(EcsWorld ecsWorld)
		{
			_ecsWorld = ecsWorld;
		}
		
		public void Process(ref int enemyEntityId)
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

			AddApplyDamageComponentToPlayer(enemyEntityId, playerEntityId);
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