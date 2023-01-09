using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using Vector2 = UnityEngine.Vector2;

namespace TonPlay.Roguelike.Client.Core.Systems
{
	public class PlayerMovementInputSystem : IEcsRunSystem, IEcsInitSystem
	{
		private const float SPEED = 4f;

		private InputControls _inputControls;
		
		public void Init(EcsSystems systems)
		{
			_inputControls = new InputControls();
			_inputControls.Player.Enable();
			_inputControls.Player.Movement.Enable();
		}
		
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var movementVector = _inputControls.Player.Movement.ReadValue<Vector2>();
			
			var world = systems.GetWorld();
			var filter = world.Filter<PlayerComponent>().Inc<MovementComponent>().End();
			var movementComponents = world.GetPool<MovementComponent>();
			var rotationComponents = world.GetPool<RotationComponent>();

			foreach (var entityId in filter) {
				ref var movementComponent = ref movementComponents.Get(entityId);
				movementComponent.Vector = movementVector * SPEED;

				ref var rotationComponent = ref rotationComponents.Get(entityId);
				
				if (movementVector.sqrMagnitude > 0.1f)
				{
					rotationComponent.Direction = movementVector.normalized;
				}
			}
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion 
		}
	}
}