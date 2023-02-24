using TonPlay.Client.Roguelike.Core.Components;
using TonPlay.Client.Roguelike.Core.Enemies.Views;
using TonPlay.Client.Roguelike.Core.Interfaces;
using TonPlay.Roguelike.Client.Utilities;
using UnityEngine;

namespace TonPlay.Client.Roguelike.Core.Actions
{
	[CreateAssetMenu(fileName = nameof(SetGameTimePauseStateAction),
		menuName = AssetMenuConstants.ACTIONS + nameof(SetGameTimePauseStateAction))]
	public class SetGameTimePauseStateAction : ScriptableAction
	{
		[SerializeField]
		private bool _state;

		public override void Execute(int callerEntityIdx, ISharedData sharedData)
		{
			var world = sharedData.World;
			var filter = world.Filter<GameComponent>().Inc<GameTimeComponent>().End();
			var pool = world.GetPool<GameTimeComponent>();

			foreach (var entityId in filter)
			{
				ref var time = ref pool.Get(entityId);
				time.Paused = _state;
			}
		}
	}
}