using TMPro;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.DefeatGame.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer.Views;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;
using UnityEngine.UI;

namespace TonPlay.Client.Roguelike.UI.Screens.DefeatGame
{
	public class DefeatGameView : View, IDefeatGameView
	{
		[SerializeField]
		private TimerView _timerView;

		[SerializeField]
		private TextMeshProUGUI _titleText;

		[SerializeField]
		private TextMeshProUGUI _newRecordText;

		[SerializeField]
		private TextMeshProUGUI _levelText;

		[SerializeField]
		private TextMeshProUGUI _bestTimeText;

		[SerializeField]
		private TextMeshProUGUI _killedEnemiesText;

		[SerializeField]
		private TextMeshProUGUI _gainedGoldText;

		[SerializeField]
		private TextMeshProUGUI _gainedProfileExperienceText;

		[SerializeField]
		private ButtonView _confirmButtonView;

		public ITimerView TimerView => _timerView;
		public IButtonView ConfirmButtonView => _confirmButtonView;

		public void SetTitleText(string text)
		{
			_titleText.SetText(text);
		}

		public void SetNewRecordText(string text)
		{
			_newRecordText.SetText(text);
		}
		
		public void SetNewRecordActiveState(bool state)
		{
			_newRecordText.gameObject.SetActive(state);
		}
		
		public void SetLevelTitleText(string text)
		{
			_levelText.SetText(text);
		}
		
		public void SetBestTimeText(string text)
		{
			_bestTimeText.SetText(text);
		}
		
		public void SetKilledEnemiesCountText(string text)
		{
			_killedEnemiesText.SetText(text);
		}
		
		public void SetGainedGoldText(string text)
		{
			_gainedGoldText.SetText(text);
		}
		
		public void SetGainedProfileExperienceText(string text)
		{
			_gainedProfileExperienceText.SetText(text);
		}
	}
}