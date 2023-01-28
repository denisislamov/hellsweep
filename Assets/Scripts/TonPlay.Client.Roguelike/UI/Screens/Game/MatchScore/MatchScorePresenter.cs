using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Game.MatchScore.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.MatchScore
{
	internal class MatchScorePresenter : Presenter<IMatchScoreView, IMatchScoreContext>
	{
		private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

		public MatchScorePresenter(
			IMatchScoreView view,
			IMatchScoreContext context)
			: base(view, context)
		{
			Init();
			AddSubscriptions();
		}

		public override void Dispose()
		{
			_compositeDisposable?.Dispose();
			base.Dispose();
		}

		private void Init()
		{
			RefreshGoldText();
			RefreshDeadEnemiesText();
		}

		private void AddSubscriptions()
		{
			Context
			   .Gold
			   .Subscribe(_ => RefreshGoldText())
			   .AddTo(_compositeDisposable);

			Context
			   .DeadEnemies
			   .Subscribe(_ => RefreshDeadEnemiesText())
			   .AddTo(_compositeDisposable);
		}

		private void RefreshGoldText()
		{
			var amount = Context.Gold.Value;

			View.SetGoldText(amount.ToString());
		}

		private void RefreshDeadEnemiesText()
		{
			var amount = Context.DeadEnemies.Value;

			View.SetDeadEnemiesText(amount.ToString());
		}

		internal class Factory : PlaceholderFactory<IMatchScoreView, IMatchScoreContext, MatchScorePresenter>
		{
		}
	}
}