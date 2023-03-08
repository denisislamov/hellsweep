using TMPro;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Rewards;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Victory.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UnityEngine;

namespace TonPlay.Client.Roguelike.UI.Screens.Victory
{
	public class VictoryView : View, IVictoryView
	{
		[SerializeField]
		private TextMeshProUGUI _titleText;
		
		[SerializeField]
		private TextMeshProUGUI _congratsText;

		[SerializeField]
		private TextMeshProUGUI _levelText;

		[SerializeField]
		private TextMeshProUGUI _killedEnemiesText;

		[SerializeField]
		private RewardItemCollectionView _rewardItemCollectionView;

		[SerializeField]
		private ButtonView _confirmButtonView;

		public IButtonView ConfirmButtonView => _confirmButtonView;
		public IRewardItemCollectionView RewardItemCollectionView => _rewardItemCollectionView;

		public void SetTitleText(string text)
		{
			_titleText.SetText(text);
		}
		
		public void SetCongratsText(string text)
		{
			_congratsText.SetText(text);
		}

		public void SetLevelTitleText(string text)
		{
			_levelText.SetText(text);
		}

		public void SetKilledEnemiesCountText(string text)
		{
			_killedEnemiesText.SetText(text);
		}
	}
}