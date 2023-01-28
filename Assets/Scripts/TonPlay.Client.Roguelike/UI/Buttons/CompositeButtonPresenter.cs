using System;
using ModestTree;
using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using Zenject;

namespace TonPlay.Client.Roguelike.UI.Buttons
{
	public class CompositeButtonPresenter : Presenter<IButtonView, ICompositeButtonContext>, IButtonPresenter
	{
		private readonly ButtonPresenterFactoryContextVisitor _factoryContextVisitor;
		public CompositeButtonPresenter(
			IButtonView view, 
			ICompositeButtonContext context,
			ButtonPresenterFactoryContextVisitor.Factory factoryContextVisitorFactory) : base(view, context)
		{
			_factoryContextVisitor = factoryContextVisitorFactory.Create(view);

			AddNestedPresenters();
		}
		
		private void AddNestedPresenters()
		{
			for (var index = 0; index < Context.ButtonContexts.Count; index++)
			{
				var buttonContext = Context.ButtonContexts[index];
				
				buttonContext.Accept(_factoryContextVisitor);
			}

			for (var index = 0; index < _factoryContextVisitor.Presenters.Count; index++)
			{
				var buttonPresenter = _factoryContextVisitor.Presenters[index];
				
				Presenters.Add(buttonPresenter);
			}
		}

		public class Factory : PlaceholderFactory<IButtonView, ICompositeButtonContext, CompositeButtonPresenter>
		{
		}
	}
}