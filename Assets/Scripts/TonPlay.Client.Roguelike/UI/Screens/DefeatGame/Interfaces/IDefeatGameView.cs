using System;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer.Views;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.DefeatGame.Interfaces
{
	public interface IDefeatGameView : IView
	{
		ITimerView TimerView { get; }

		IButtonView ConfirmButtonView { get; }

		void SetTitleText(string text);

		void SetNewRecordText(string text);

		void SetNewRecordActiveState(bool state);

		void SetLevelTitleText(string text);

		void SetBestTimeText(string text);

		void SetKilledEnemiesCountText(string text);

		void SetGainedGoldText(string text);

		void SetGainedProfileExperienceText(string text);
	}
}