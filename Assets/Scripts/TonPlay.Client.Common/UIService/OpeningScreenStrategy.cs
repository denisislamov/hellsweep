using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Common.UIService
{
	public abstract class OpeningScreenStrategy : IOpeningScreenStrategy
	{
		public TScreen Open<TScreen, TContext>(TContext context) 
			where TScreen : IScreen 
			where TContext : IScreenContext
		{
			var screen = Create<TScreen, TContext>(context);
			screen.Open();
			return screen;
		}
		
		protected abstract TScreen Create<TScreen, TContext>(TContext context)
			where TScreen : IScreen
			where TContext : IScreenContext;
	}
}