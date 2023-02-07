using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
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
		
		public void Process(ref int utilityEntityId)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
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

			AddApplyDamageComponentToPlayer(utilityEntityId, playerEntityId);
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion
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
				playerApplyDamageComponent.Damage += damageComponent.DamageProvider.Damage;
			}
			else
			{
				ref var playerApplyDamageComponent = ref applyDamageComponents.Add(playerEntityId);
				playerApplyDamageComponent.Damage = damageComponent.DamageProvider.Damage;
			}
		}
	}
}