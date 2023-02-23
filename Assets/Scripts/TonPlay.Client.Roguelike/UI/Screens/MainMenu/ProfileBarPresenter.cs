using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.Models.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.MainMenu.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.MainMenu
{
	public class ProfileBarPresenter : Presenter<IProfileBarView, IProfileBarContext>
	{
		private readonly IProfileModel _profileModel;

		private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

		public ProfileBarPresenter(
			IProfileBarView view,
			IProfileBarContext context,
			IMetaGameModelProvider metaGameModelProvider)
			: base(view, context)
		{
			_profileModel = metaGameModelProvider.Get().ProfileModel;

			AddSubscriptions();
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
		}

		private void UpdateEnergyInfo(int value)
		{
			View.SetEnergyText($"{_profileModel.BalanceModel.Energy.Value}/{_profileModel.BalanceModel.MaxEnergy.Value}");
		}

		private void UpdateGoldInfo(int value)
		{
			View.SetGoldText($"{_profileModel.BalanceModel.Gold.Value}");
		}

		private void UpdateLevelInfo(int value)
		{
			View.SetLevelText($"{_profileModel.Level.Value}");
		}

		private void UpdateExperienceInfo(float value)
		{
			var progressBarValue = _profileModel.Experience.Value/_profileModel.MaxExperience.Value;
			View.ExperienceProgressBarView.SetSize(progressBarValue);
		}

		public class Factory : PlaceholderFactory<IProfileBarView, IProfileBarContext, ProfileBarPresenter>
		{
		}
	}
}