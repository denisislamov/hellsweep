using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.MatchScore.Interfaces
{
	public interface IMatchScoreContext : IScreenContext
	{
		IReadOnlyReactiveProperty<int> Gold { get; }

		IReadOnlyReactiveProperty<int> DeadEnemies { get; }
	}
}