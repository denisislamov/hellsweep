using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies.TerracottaHorseman;
using TonPlay.Client.Roguelike.Core.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Systems.Enemies.TerracottaHorseman
{
	public class TerracottaHorsemanAnimatorSystem : IEcsRunSystem
	{
		private static readonly int s_Attack = Animator.StringToHash("Attack");
		
		public void Run(EcsSystems systems)
		{
			var sharedData = systems.GetShared<ISharedData>();
			var world = systems.GetWorld();
			var filter = world.Filter<TerracottaHorsemanComponent>()
							  .Inc<PositionComponent>()
							  .Inc<AnimatorComponent>()
							  .Exc<DeadComponent>()
							  .End();
			var horsemanPool = world.GetPool<TerracottaHorsemanComponent>();
			var animatorPool = world.GetPool<AnimatorComponent>();
			var positionPool = world.GetPool<PositionComponent>();

			foreach (var entityId in filter)
			{
				ref var position = ref positionPool.Get(entityId);
				ref var horseman = ref horsemanPool.Get(entityId);
				ref var animator = ref animatorPool.Get(entityId);

				var previousAttackingState = horseman.IsAttacking;

				horseman.IsAttacking = Vector2.Distance(position.Position, sharedData.PlayerPositionProvider.Position) <= horseman.AttackAnimDistance;

				if (previousAttackingState != horseman.IsAttacking)
				{
					animator.Animator.SetBool(s_Attack, horseman.IsAttacking);
				}
			}
		}
	}
}