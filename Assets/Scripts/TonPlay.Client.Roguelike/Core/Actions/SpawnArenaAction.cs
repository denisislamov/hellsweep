using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Components.Enemies;
using TonPlay.Client.Roguelike.Core.Enemies.Views;
using TonPlay.Client.Roguelike.Core.Interfaces;
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

		public override void Execute(int callerEntityIdx, ISharedData sharedData)
		{
			var world = sharedData.World;
			var positionPool = world.GetPool<PositionComponent>();
			var callerPosition = positionPool.Get(callerEntityIdx);

			var view = Instantiate(_arenaView);
			var transform = view.transform;
			transform.position = callerPosition.Position;

			var entity = world.NewEntity();

			ref var arena = ref entity.Add<Arena>();
			arena.View = view;

			ref var nonPoolObj = ref entity.Add<GameNonPoolObject>();
			nonPoolObj.GameObject = view.gameObject;
		}
	}
}