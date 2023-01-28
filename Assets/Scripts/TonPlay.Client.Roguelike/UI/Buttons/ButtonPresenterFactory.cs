using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Buttons
{
	public class ButtonPresenterFactory : IButtonPresenterFactory
	{
		private readonly CompositeButtonPresenter.Factory _compositeButtonPresenterFactory;

		public ButtonPresenterFactory(
			CompositeButtonPresenter.Factory compositeButtonPresenterFactory)
		{
			_compositeButtonPresenterFactory = compositeButtonPresenterFactory;
		}

		public IButtonPresenter Create(IButtonView view, ICompositeButtonContext context)
		{
			return _compositeButtonPresenterFactory.Create(view, context);
		}
	}
}