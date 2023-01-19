using TonPlay.Client.Roguelike.Core.Models.Data;
using TonPlay.Roguelike.Client.Core.Models.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.Core.Models.Interfaces
{
	public interface IGameModel
	{
		IPlayerModel PlayerModel { get; }
		
		IReadOnlyReactiveProperty<float> GameTime { get; }
		
		IReadOnlyReactiveProperty<bool> Paused { get; }
		
		IReadOnlyReactiveProperty<int> DeadEnemiesCount { get; }

		void Update(GameData data);

		GameData ToData();
	}
}