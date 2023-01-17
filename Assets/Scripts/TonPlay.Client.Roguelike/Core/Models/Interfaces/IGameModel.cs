using TonPlay.Roguelike.Client.Core.Models.Data;
using UniRx;

namespace TonPlay.Roguelike.Client.Core.Models.Interfaces
{
	public interface IGameModel
	{
		IPlayerModel PlayerModel { get; }
		
		IReadOnlyReactiveProperty<float> GameTime { get; }
		
		IReadOnlyReactiveProperty<bool> Paused { get; }

		void Update(GameData data);

		GameData ToData();
	}
}