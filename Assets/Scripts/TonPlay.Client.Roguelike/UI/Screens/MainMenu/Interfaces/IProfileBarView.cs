using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces
{
	public interface IProfileBarView : IView
	{
		IProgressBarView ExperienceProgressBarView { get; }
		
		IButtonView GameSettingsButtonView { get; }

		void SetLevelText(string text);

		void SetEnergyText(string text);

		void SetGoldText(string text);

		void SetUsername(string text);
	}
}