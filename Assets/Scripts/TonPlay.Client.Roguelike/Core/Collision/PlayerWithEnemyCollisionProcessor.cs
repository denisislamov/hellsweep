using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Roguelike.Client.Core.Collision.Interfaces;

namespace TonPlay.Client.Roguelike.Core.Collision
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
// TonPlay.Client.Common.Utilities.ProfilingTool.BeginSample(this);
// 			var players = 
// 				_ecsWorld.Filter<PlayerComponent>()
// 						 .Inc<HealthComponent>()
// 						 .End();
//
// 			var playerEntityId = EcsEntity.DEFAULT_ID;
// 			foreach (var entityId in players)
// 			{
// 				playerEntityId = entityId;
// 			}
//
// 			if (playerEntityId == EcsEntity.DEFAULT_ID || !_ecsWorld.IsEntityAlive(playerEntityId))
// 			{
// 				return;
// 			}
//
// 			AddApplyDamageComponentToPlayer(utilityEntityId, playerEntityId);
// TonPlay.Client.Common.Utilities.ProfilingTool.EndSample();
		}

		private void AddApplyDamageComponentToPlayer(
			int enemyEntityId,
			int playerEntityId)
		{
			// var damageDataComponents = _ecsWorld.GetPool<DamageOnCollisionComponent>();
			// var stackPool = _ecsWorld.GetPool<StackTryApplyDamageComponent>();
			//
			// ref var damageComponent = ref damageDataComponents.Get(enemyEntityId);
			//
			// if (stackPool.Has(enemyEntityId))
			// {
			// 	ref var enemyStack = ref stackPool.Get(enemyEntityId);
			// 	enemyStack.Damage += damageComponent.DamageProvider.Damage;
			// 	enemyStack.DamageSourceEntityId = enemyEntityId;
			// }
			// else
			// {
			// 	ref var enemyStack = ref stackPool.Add(enemyEntityId);
			// 	enemyStack.Damage = damageComponent.DamageProvider.Damage;
			// 	enemyStack.DamageSourceEntityId = enemyEntityId;
			// }
		}
	}
}