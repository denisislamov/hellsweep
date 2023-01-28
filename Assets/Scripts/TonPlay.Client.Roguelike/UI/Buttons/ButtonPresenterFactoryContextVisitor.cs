using System.Collections.Generic;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Buttons
{
	public class ButtonPresenterFactoryContextVisitor : IButtonContextVisitor
	{
		private readonly TextButtonPresenter.Factory _textButtonPresenterFactory;
		private readonly ClickableButtonPresenter.Factory _clickableButtonPresenterFactory;
		private readonly ReactiveTextButtonPresenter.Factory _reactiveTextButtonPresenterFactory;

		private readonly IButtonView _buttonView;

		private readonly List<IButtonPresenter> _presenters = new List<IButtonPresenter>();

		public IReadOnlyList<IButtonPresenter> Presenters => _presenters;

		public ButtonPresenterFactoryContextVisitor(
			IButtonView buttonView,
			TextButtonPresenter.Factory textButtonPresenterFactory,
			ClickableButtonPresenter.Factory clickableButtonPresenterFactory,
			ReactiveTextButtonPresenter.Factory reactiveTextButtonPresenterFactory)
		{
			_buttonView = buttonView;
			_textButtonPresenterFactory = textButtonPresenterFactory;
			_clickableButtonPresenterFactory = clickableButtonPresenterFactory;
			_reactiveTextButtonPresenterFactory = reactiveTextButtonPresenterFactory;
		}

		public void Visit(IClickableButtonContext context)
		{
			var presenter = _clickableButtonPresenterFactory.Create(_buttonView, context);

			_presenters.Add(presenter);
		}

		public void Visit(IReactiveTextButtonContext context)
		{
			var presenter = _reactiveTextButtonPresenterFactory.Create(_buttonView, context);

			_presenters.Add(presenter);
		}

		public void Visit(ITextButtonContext context)
		{
			var presenter = _textButtonPresenterFactory.Create(_buttonView, context);

			_presenters.Add(presenter);
		}

		public class Factory : PlaceholderFactory<IButtonView, ButtonPresenterFactoryContextVisitor>
		{
		}
	}
}