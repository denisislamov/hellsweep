using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer.Views;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Victory.Interfaces
{
	public interface IVictoryView : IView
	{
		IButtonView ConfirmButtonView { get; }
		
		IRewardItemCollectionView RewardItemCollectionView { get; }

		void SetTitleText(string text);
		
		void SetCongratsText(string text);

		void SetLevelTitleText(string text);
		
		void SetKilledEnemiesCountText(string text);
	}
}