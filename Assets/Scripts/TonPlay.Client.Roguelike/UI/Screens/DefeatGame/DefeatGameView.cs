using TMPro;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Rewards;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
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
		private RewardItemCollectionView _rewardItemCollectionView;

		[SerializeField]
		private ButtonView _confirmButtonView;

		[SerializeField]
		private GameObject[] _newRecordObjects;

		public ITimerView TimerView => _timerView;
		public IButtonView ConfirmButtonView => _confirmButtonView;
		public IRewardItemCollectionView RewardItemCollectionView => _rewardItemCollectionView;

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
			for (var i = 0; i < _newRecordObjects.Length; i++)
			{
				var newRecordObject = _newRecordObjects[i];
				
				newRecordObject.SetActive(state);
			}
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
	}
}