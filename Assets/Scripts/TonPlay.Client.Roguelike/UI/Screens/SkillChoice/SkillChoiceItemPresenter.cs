using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Interfaces;
using TonPlay.Client.Roguelike.UI.Screens.SkillChoice.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.Screens.SkillChoice.Views.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Screens.SkillChoice
{
	internal class SkillChoiceItemPresenter : Presenter<ISkillChoiceItemView, ISkillChoiceItemContext>
	{
		private IDisposable _disposable;

		public SkillChoiceItemPresenter(
			ISkillChoiceItemView view,
			ISkillChoiceItemContext context)
			: base(view, context)
		{
			InitView();
			AddSubscription();
		}

		public override void Dispose()
		{
			_disposable?.Dispose();
			base.Dispose();
		}

		private void AddSubscription()
		{
			_disposable = View.OnButtonClick.Subscribe(_ => Context.ClickedCallback?.Invoke(Context.SkillName));
		}

		private void InitView()
		{
			var nextLevel = Context.CurrentLevel + 1 <= Context.MaxLevel
				? Context.CurrentLevel + 1
				: Context.MaxLevel;

			View.SetIcon(Context.Icon);
			View.SetTitleText(Context.Title);
			View.SetDescriptionText(Context.Description);
			View.SetCurrentLevel(Context.CurrentLevel);
			View.SetMaxLevel(Context.MaxLevel);
			View.SetNextLevel(nextLevel);
			View.SetBackgroundColor(Context.Color);
			View.SetTitleTextColor(Context.Color);
			View.SetLevelIcon(Context.LevelIcon);
		}

		internal class Factory : PlaceholderFactory<ISkillChoiceItemView, ISkillChoiceItemContext, SkillChoiceItemPresenter>
		{
		}
	}
}