using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.MatchScore.Interfaces
{
	public interface IMatchScoreView : IView
	{
		void SetGoldText(string text);

		void SetDeadEnemiesText(string text);
	}
}