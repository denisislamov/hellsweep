using Leopotam.EcsLite;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies.BossButcher;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Weapons.Configs;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Actions
{
	[CreateAssetMenu(fileName = nameof(SetPositionNearToThePlayerAction),
		menuName = AssetMenuConstants.ACTIONS + "Common/" + nameof(SetPositionNearToThePlayerAction))]
	public class SetPositionNearToThePlayerAction : ScriptableAction
	{
		[SerializeField]
		private Vector2 _offsetPos;

		public override void Execute(int callerEntityIdx, ISharedData sharedData)
		{
			var entity = new EcsEntity(sharedData.MainWorld, callerEntityIdx);

			ref var positionComponent = ref entity.Get<PositionComponent>();

			var targetPosition = sharedData.PlayerPositionProvider.Position + _offsetPos;

			if (entity.Has<TransformComponent>())
			{
				ref var transformComponent = ref entity.Get<TransformComponent>();
				transformComponent.Transform.position = targetPosition;
				positionComponent.Position = targetPosition;
			} 
			else if (entity.Has<RigidbodyComponent>())
			{
				ref var rigidbodyComponent = ref entity.Get<RigidbodyComponent>();
				rigidbodyComponent.Rigidbody.transform.position = sharedData.PlayerPositionProvider.Position;
				rigidbodyComponent.Rigidbody.MovePosition(targetPosition);
				positionComponent.Position = rigidbodyComponent.Rigidbody.position;
			}
		}
	}
}