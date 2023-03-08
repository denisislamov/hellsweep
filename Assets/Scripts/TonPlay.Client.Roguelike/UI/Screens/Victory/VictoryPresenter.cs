using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Core.Match.Interfaces;
using TonPlay.Client.Roguelike.Core.Models.Interfaces;
using TonPlay.Client.Roguelike.Models.Data;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.Profile.Interfaces;
using TonPlay.Client.Roguelike.SceneService.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Rewards;
using TonPlay.Client.Roguelike.UI.Rewards.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.Victory.Interfaces;
using TonPlay.Client.Roguelike.Utilities;
using TonPlay.Roguelike.Client.UI.UIService.Layers;
using TonPlay.Roguelike.Client.Utilities;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Victory
{
	internal class VictoryPresenter : Presenter<IVictoryView, IVictoryScreenContext>
	{
		private readonly IGameModelProvider _gameModelProvider;
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly IUIService _uiService;
		private readonly IMatchProvider _matchProvider;
		private readonly ILocationConfigStorage _locationConfigStorage;
		private readonly RewardItemCollectionPresenter.Factory _rewardItemCollectionPresenterFactory;
		private bool _loading;

		public VictoryPresenter(
			IVictoryView view,
			IVictoryScreenContext context,
			IGameModelProvider gameModelProvider,
			IButtonPresenterFactory buttonPresenterFactory,
			IUIService uiService,
			IMatchProvider matchProvider,
			ILocationConfigStorage locationConfigStorage,
			RewardItemCollectionPresenter.Factory rewardItemCollectionPresenterFactory)
			: base(view, context)
		{
			_gameModelProvider = gameModelProvider;
			_buttonPresenterFactory = buttonPresenterFactory;
			_uiService = uiService;
			_matchProvider = matchProvider;
			_locationConfigStorage = locationConfigStorage;
			_rewardItemCollectionPresenterFactory = rewardItemCollectionPresenterFactory;

			InitView();
			AddButtonPresenter();
			AddRewardItemCollectionPresenter();
		}

		private void InitView()
		{
			var gameModel = _gameModelProvider.Get();

			View.SetTitleText("Victory");
			View.SetCongratsText("Congratulations!");
			
			View.SetLevelTitleText(_locationConfigStorage.Current.Title);
			View.SetKilledEnemiesCountText(string.Format("Enemies defeated: <size=150%>{0}</size>", gameModel.DeadEnemiesCount));
		}

		private void AddButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(View.ConfirmButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(ConfirmButtonClickHandler))
				   .Add(new TextButtonContext("Confirm")));

			Presenters.Add(presenter);
		}
		
		private void AddRewardItemCollectionPresenter()
		{
			var rewardList = new List<IRewardData>();
			var gameModel = _gameModelProvider.Get();
			var gainModel = gameModel.PlayerModel.MatchProfileGainModel;

			if (gainModel.Gold.Value > 0)
			{
				rewardList.Add(new RewardData(RoguelikeConstants.Core.Rewards.COINS_ID, gainModel.Gold.Value));
			}
			
			if (gainModel.ProfileExperience.Value > 0)
			{
				rewardList.Add(new RewardData(RoguelikeConstants.Core.Rewards.PROFILE_EXPERIENCE_ID, (int) gainModel.ProfileExperience.Value));
			}
			
			var presenter = _rewardItemCollectionPresenterFactory.Create(
				View.RewardItemCollectionView, 
				new RewardItemCollectionContext(rewardList));
			
			Presenters.Add(presenter);
		}

		private void ConfirmButtonClickHandler()
		{
			if (_loading)
			{
				return;
			}

			_loading = true;
			_uiService.CloseAll(new DefaultScreenLayer());

			_matchProvider.Current.Finish();
		}
		
		internal class Factory : PlaceholderFactory<IVictoryView, IVictoryScreenContext, VictoryPresenter>
		{
		}
	}
}