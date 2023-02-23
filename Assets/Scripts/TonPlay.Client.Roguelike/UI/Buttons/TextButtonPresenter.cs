using System;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Buttons
{
	public class TextButtonPresenter : Presenter<IButtonView, ITextButtonContext>, IButtonPresenter
	{
		public TextButtonPresenter(
			IButtonView view,
			ITextButtonContext context) : base(view, context)
		{
			ChangeViewText();
		}

		private void ChangeViewText()
		{
			View.SetText(Context.Text);
		}

		public class Factory : PlaceholderFactory<IButtonView, ITextButtonContext, TextButtonPresenter>
		{
		}
	}
}