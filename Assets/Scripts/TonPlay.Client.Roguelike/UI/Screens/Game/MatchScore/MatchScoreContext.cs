using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Game.MatchScore.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.MatchScore
{
	public class MatchScoreContext : ScreenContext, IMatchScoreContext
	{
		public IReadOnlyReactiveProperty<int> Gold { get; }
		public IReadOnlyReactiveProperty<int> DeadEnemies { get; }

		public MatchScoreContext(IReadOnlyReactiveProperty<int> gold, IReadOnlyReactiveProperty<int> deadEnemies)
		{
			Gold = gold;
			DeadEnemies = deadEnemies;
		}
	}
}