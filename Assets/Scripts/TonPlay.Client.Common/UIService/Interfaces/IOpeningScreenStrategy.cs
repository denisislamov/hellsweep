using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Common.UIService.Interfaces
{
	public interface IOpeningScreenStrategy
	{
		void Open<TScreen, TContext>(TContext context)
			where TContext : IScreenContext
			where TScreen : IScreen;
	}
}