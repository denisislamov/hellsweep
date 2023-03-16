using TonPlay.Client.Roguelike.Core.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Core.Models.Interfaces
{
	public interface IGameModel : IModel<GameData>
	{
		IPlayerModel PlayerModel { get; }
		
		IBossModel BossModel { get; }

		IReadOnlyReactiveProperty<float> GameTime { get; }

		IReadOnlyReactiveProperty<bool> Paused { get; }

		IReadOnlyReactiveProperty<int> DeadEnemiesCount { get; }
		
		IReadOnlyReactiveProperty<int> DebugEnemyMovementToEachOtherCollisionCount { get; }
	}
}