using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class PlayerInputSystem : IEcsRunSystem, IEcsInitSystem
	{
		private const float SPEED = 2f;

		private InputControls _inputControls;
		
		public void Init(EcsSystems systems)
		{
			_inputControls = new InputControls();
			_inputControls.Player.Enable();
			_inputControls.Player.Movement.Enable();
		}
		
		public void Run(EcsSystems systems)
		{
			var movementVector = _inputControls.Player.Movement.ReadValue<Vector2>();

			if (movementVector.sqrMagnitude < 0.1f) return;
			
			var world = systems.GetWorld();
			var filter = world.Filter<PlayerComponent>().End();
			var movementComponents = world.GetPool<MovementComponent>();

			foreach (var entityId in filter) {
				ref var movementComponent = ref movementComponents.Add(entityId);
				movementComponent.Vector = movementVector * SPEED;
			}
		}
	}
}