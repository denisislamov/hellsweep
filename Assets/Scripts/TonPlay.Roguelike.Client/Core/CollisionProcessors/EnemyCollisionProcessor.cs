using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.CollisionProcessors.Interfaces;
using TonPlay.Roguelike.Client.Core.Components;

namespace TonPlay.Roguelike.Client.Core.CollisionProcessors
{
	public class EnemyCollisionProcessor : ICollisionProcessor
	{
		private readonly EcsWorld _ecsWorld;
		
		public EnemyCollisionProcessor(EcsWorld ecsWorld)
		{
			_ecsWorld = ecsWorld;
		}
		
		public void Process(ref CollisionEventComponent collisionEventComponent)
		{
			var players = _ecsWorld.Filter<PlayerComponent>().Inc<HealthComponent>().End();
			var enemies = _ecsWorld.Filter<EnemyComponent>().Inc<RigidbodyComponent>().Inc<DamageOnCollisionComponent>().End();
			
			var rigidbodyComponents = _ecsWorld.GetPool<RigidbodyComponent>();
			var damageDataComponents = _ecsWorld.GetPool<DamageOnCollisionComponent>();
			var applyDamageComponents = _ecsWorld.GetPool<ApplyDamageComponent>();

			var playerEntityId = EcsEntity.DEFAULT_ID;
			foreach (var entityId in players)
			{
				playerEntityId = entityId;
			}

			if (playerEntityId == EcsEntity.DEFAULT_ID)
			{
				return;
			}

			foreach (var entityId in enemies)
			{
				ref var rigidbodyComponent = ref rigidbodyComponents.Get(entityId);
				if (rigidbodyComponent.Rigidbody != collisionEventComponent.CollidedRigidbody)
				{
					continue;
				}
				
				AddApplyDamageComponentToPlayer(damageDataComponents, entityId, applyDamageComponents, playerEntityId);
			}
		}
		
		private static void AddApplyDamageComponentToPlayer(
			EcsPool<DamageOnCollisionComponent> damageDataComponents, 
			int entityId, 
			EcsPool<ApplyDamageComponent> applyDamageComponents, 
			int playerEntityId)
		{
			ref var damageComponent = ref damageDataComponents.Get(entityId);
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