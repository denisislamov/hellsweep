using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public abstract class OpeningScreenStrategy : IOpeningScreenStrategy
	{
		public void Open<TScreen, TContext>(TContext context) 
			where TScreen : IScreen 
			where TContext : IScreenContext
		{
			var screen = Create<TScreen, TContext>(context);
			screen.Open();
		}
		
		protected abstract TScreen Create<TScreen, TContext>(TContext context)
			where TScreen : IScreen
			where TContext : IScreenContext;
	}
}