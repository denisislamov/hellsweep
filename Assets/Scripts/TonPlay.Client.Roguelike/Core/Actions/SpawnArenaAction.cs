using TonPlay.Client.Roguelike.Core.Collision;
using TonPlay.Client.Roguelike.Core.Collision.Interfaces;
using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies;
using TonPlay.Client.Roguelike.Core.Enemies.Views;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Client.Roguelike.Extensions;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Actions
{
	[CreateAssetMenu(fileName = nameof(SpawnArenaAction),
		menuName = AssetMenuConstants.ACTIONS + nameof(SpawnArenaAction))]
	public class SpawnArenaAction : ScriptableAction
	{
		[SerializeField]
		private ArenaView _arenaView;

		[SerializeField]
		private CollisionAreaConfig _collisionAreaConfig;

		[SerializeField, Layer]
		private int _layer;

		[SerializeField]
		private LayerMask _layerMask;

		public override void Execute(int callerEntityIdx, ISharedData sharedData)
		{
			var world = sharedData.MainWorld;
			var positionPool = world.GetPool<PositionComponent>();
			var callerPosition = positionPool.Get(callerEntityIdx);

			var view = Instantiate(_arenaView);
			var transform = view.transform;
			var position = callerPosition.Position;
			transform.position = position;

			var entity = world.NewEntity();

			ref var arena = ref entity.Add<Arena>();
			arena.View = view;

			entity.AddCollisionComponent(CollisionAreaFactory.Create(_collisionAreaConfig), _layerMask);
			entity.AddLayerComponent(_layer);
			entity.AddHasCollidedComponent();
			entity.AddPositionComponent(position);

			sharedData.ArenasKdTreeStorage.AddEntity(entity.Id, position);

			ref var nonPoolObj = ref entity.Add<GameNonPoolObject>();
			nonPoolObj.GameObject = view.gameObject;
		}
	}
}