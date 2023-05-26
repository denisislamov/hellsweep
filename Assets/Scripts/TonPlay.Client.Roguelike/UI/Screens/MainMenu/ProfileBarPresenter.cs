using TonPlay.Client.Common.UIService;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Buttons;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.GameSettings;
using TonPlay.Client.Roguelike.UI.Screens.GameSettings.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MyBag;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu
{
	public class ProfileBarPresenter : Presenter<IProfileBarView, IProfileBarContext>
	{
		private readonly IButtonPresenterFactory _buttonPresenterFactory;
		private readonly IUIService _uiService;
		private readonly IProfileModel _profileModel;

		private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

		public ProfileBarPresenter(
			IProfileBarView view,
			IProfileBarContext context,
			IMetaGameModelProvider metaGameModelProvider,
			IButtonPresenterFactory buttonPresenterFactory,
			IUIService uiService)
			: base(view, context)
		{
			_buttonPresenterFactory = buttonPresenterFactory;
			_uiService = uiService;
			_profileModel = metaGameModelProvider.Get().ProfileModel;

			AddSubscriptions();
			AddSettingsButtonPresenter();
		}

		public override void Dispose()
		{
			_subscriptions?.Dispose();

			base.Dispose();
		}

		private void AddSubscriptions()
		{
			_profileModel.BalanceModel.Energy.Subscribe(UpdateEnergyInfo).AddTo(_subscriptions);
			_profileModel.BalanceModel.MaxEnergy.Subscribe(UpdateEnergyInfo).AddTo(_subscriptions);

			_profileModel.BalanceModel.Gold.Subscribe(UpdateGoldInfo).AddTo(_subscriptions);

			_profileModel.Level.Subscribe(UpdateLevelInfo).AddTo(_subscriptions);

			_profileModel.Experience.Subscribe(UpdateExperienceInfo).AddTo(_subscriptions);
			_profileModel.MaxExperience.Subscribe(UpdateExperienceInfo).AddTo(_subscriptions);

			_profileModel.Username.Subscribe(UpdateUsername).AddTo(_subscriptions);
		}

		private void UpdateEnergyInfo(long value)
		{
			View.SetEnergyText($"{_profileModel.BalanceModel.Energy.Value}/{_profileModel.BalanceModel.MaxEnergy.Value}");
		}

		private void UpdateGoldInfo(long value)
		{
			View.SetGoldText($"{_profileModel.BalanceModel.Gold.Value}");
		}

		private void UpdateLevelInfo(int value)
		{
			View.SetLevelText($"{_profileModel.Level.Value}");
		}

		private void UpdateUsername(string value)
		{
			View.SetUsername($"{_profileModel.Username.Value}");
		}
		
		private void UpdateExperienceInfo(float value)
		{
			var progressBarValue = _profileModel.Experience.Value/_profileModel.MaxExperience.Value;
			View.ExperienceProgressBarView.SetSize(progressBarValue);
		}

		private void AddSettingsButtonPresenter()
		{
			var presenter = _buttonPresenterFactory.Create(View.GameSettingsButtonView,
				new CompositeButtonContext()
				   .Add(new ClickableButtonContext(OnSettingsButtonClickHandler)));

			Presenters.Add(presenter);
		}
		
		private void OnSettingsButtonClickHandler()
		{
			_uiService.Open<MyBagPopupScreen, MyBagPopupScreenContext>(new MyBagPopupScreenContext());
		}
		
		
		public class Factory : PlaceholderFactory<IProfileBarView, IProfileBarContext, ProfileBarPresenter>
		{
		}
	}
}