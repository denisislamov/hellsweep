using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar.Interfaces
{
	public interface ILevelProgressBarView : IView, IProgressBarView
	{
		void SetLevelText(string text);
	}
}