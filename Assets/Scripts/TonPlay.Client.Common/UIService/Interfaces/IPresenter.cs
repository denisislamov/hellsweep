using System;
using TonPlay.Client.Common.UIService.Interfaces;

namespace TonPlay.Roguelike.Client.UI.UIService.Interfaces
{
	public interface IPresenter : IDisposable
	{
		void Show();
		
		void Hide();
	}

	public interface IPresenter<TView, TContext> : IPresenter
		where TView : IView
		where TContext : IScreenContext
	{
		TView View { get; }
		
		TContext Context { get; }
	}
}