using System.Collections.Generic;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public class Presenter<TView, TContext> : IPresenter<TView, TContext>
		where TView : IView
		where TContext : IScreenContext
	{
		public TView View { get; }
		
		public TContext Context { get; }

		protected List<IPresenter> Presenters { get; } = new ();
		
		public Presenter(
			TView view, 
			TContext context)
		{
			View = view;
			Context = context;
		}
		
		public void Show()
		{
			View.Show();
			
			foreach (var presenter in Presenters)
			{
				presenter.Show();
			}
		}
		
		public void Hide()
		{
			foreach (var presenter in Presenters)
			{
				presenter.Hide();
			}
			
			View.Hide();
		}
	}
}