using System.Collections.Generic;
using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Common.UIService
{
	public class Presenter<TView, TContext> : IPresenter<TView, TContext>
		where TView : IView
		where TContext : IScreenContext
	{
		public TView View { get; }
		
		public TContext Context { get; }

		protected List<IPresenter> Presenters { get; } = new List<IPresenter>();
		
		public Presenter(
			TView view, 
			TContext context)
		{
			View = view;
			Context = context;
		}
		
		public virtual void Show()
		{
			View.Show();
			
			foreach (var presenter in Presenters)
			{
				presenter.Show();
			}
		}
		
		public virtual void Hide()
		{
			foreach (var presenter in Presenters)
			{
				presenter.Hide();
			}
			
			View.Hide();
		}
		
		public virtual void Dispose()
		{
			foreach (var presenter in Presenters)
			{
				presenter.Dispose();
			}
		}
	}
}