using Leopotam.EcsLite;
using Leopotam.EcsLite.Extensions;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Effects;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Core.Pooling.Identities;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Actions
{
	[CreateAssetMenu(fileName = nameof(SpawnEffectAction),
		menuName = AssetMenuConstants.ACTIONS + "Common/" + nameof(SpawnEffectAction))]
	public class SpawnEffectAction : ScriptableAction
	{
		[SerializeField] 
		private EffectView _effectView;

		[SerializeField]
		private Vector2 _spawnOffset;

		[SerializeField]
		private bool _syncDestroyWithCaller;

		[SerializeField]
		private bool _syncPositionWithCaller;
		
		[SerializeField]
		private bool _syncRotationWithCaller;
		
		[SerializeField]
		private bool _syncRotationWithMovementDifference;

		public override void Execute(int callerEntityIdx, ISharedData sharedData)
		{
			var positionPool = sharedData.MainWorld.GetPool<PositionComponent>();
			var rotationPool = sharedData.MainWorld.GetPool<RotationComponent>();
			
			var identity = new GameObjectViewPoolIdentity(_effectView.gameObject);
			if (!sharedData.CompositeViewPool.TryGet<EffectView>(identity, out var viewPoolObject))
			{
				return;
			}

			var view = viewPoolObject.Object;
			var effectEntity = sharedData.EffectsWorld.NewEntity();
			viewPoolObject.EntityId = effectEntity.Id;

			effectEntity.AddEffectComponent();
			effectEntity.AddRotationComponent(Vector2.zero);
			effectEntity.AddTransformComponent(view.transform);
			effectEntity.AddPoolObjectComponent(viewPoolObject);

			ref var position = ref positionPool.AddOrGet(effectEntity.Id);

			if (view.PlayableDirector != null)
			{
				view.PlayableDirector.Stop();
				view.PlayableDirector.Play();
				effectEntity.AddPlayableDirectorComponent(view.PlayableDirector);
			}

			if (positionPool.Has(callerEntityIdx) && _syncPositionWithCaller)
			{
				ref var callerPosition = ref positionPool.Get(callerEntityIdx);
				var targetPosition = callerPosition.Position + _spawnOffset;
				view.transform.position = targetPosition;

				position.Position = targetPosition;

				ref var syncRotation = ref effectEntity.Add<SyncPositionWithAnotherEntityComponent>();
				syncRotation.ParentEntityId = callerEntityIdx;
				syncRotation.ParentWorld = sharedData.MainWorld;
			}

			if (rotationPool.Has(callerEntityIdx) && _syncRotationWithCaller)
			{
				ref var syncRotation = ref effectEntity.Add<SyncRotationWithAnotherEntityComponent>();
				
				syncRotation.ParentEntityId = callerEntityIdx;
				syncRotation.ParentWorld = sharedData.MainWorld;
			}

			if (_syncDestroyWithCaller)
			{
				var callerEntity = new EcsEntity(sharedData.MainWorld, callerEntityIdx);
				ref var syncDestroy = ref callerEntity.Add<SyncDestroyWithAnotherEntityComponent>();
				syncDestroy.NextEntityId = effectEntity.Id;
				syncDestroy.NextEntityWorld = sharedData.EffectsWorld;
			}

			if (_syncRotationWithMovementDifference)
			{
				ref var syncRotation = ref effectEntity.Add<SyncRotationWithPositionDifferenceComponent>();
			}
		}
	}
}