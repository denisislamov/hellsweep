using Leopotam.EcsLite;
using TonPlay.Roguelike.Client.Core.Components;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace TonPlay.Client.Roguelike.Core.Systems
{
	public class PlayerMovementInputSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
	{
		private DefaultInputActions _inputControls;
		
		public void Init(EcsSystems systems)
		{
			_inputControls = new DefaultInputActions();
			_inputControls.Player.Enable();
			_inputControls.Player.Move.Enable();
		}
		
		public void Run(EcsSystems systems)
		{
#region Profiling Begin
			UnityEngine.Profiling.Profiler.BeginSample(GetType().FullName);
#endregion
			var movementVector = _inputControls.Player.Move.ReadValue<Vector2>();
			
			var world = systems.GetWorld();
			var filter = world.Filter<PlayerComponent>().Inc<MovementComponent>().End();
			var movementComponents = world.GetPool<MovementComponent>();
			var rotationComponents = world.GetPool<RotationComponent>();

			foreach (var entityId in filter) {
				ref var movementComponent = ref movementComponents.Get(entityId);
				movementComponent.Vector = movementVector;

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
		
		public void Destroy(EcsSystems systems)
		{
			_inputControls.Disable();
			_inputControls.Dispose();
		}
	}
}