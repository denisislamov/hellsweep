namespace TonPlay.Roguelike.Client.UI.UIService.Interfaces
{
	public interface IPresenter
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