using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public class EmbeddedOpeningScreenStrategy : OpeningScreenStrategy
	{
		private readonly IScreenFactoryFacade _screenFactoryFacade;
		private readonly IScreenStack _screensStack;
		private readonly IScreenLayer _layer;
		
		public EmbeddedOpeningScreenStrategy(IScreenFactoryFacade screenFactoryFacade, IScreenLayer layer, IScreenStack screensStack)
		{
			_screenFactoryFacade = screenFactoryFacade;
			_layer = layer;
			_screensStack = screensStack;
		}
		
		protected override TScreen Create<TScreen, TContext>(TContext context) 
		{
			var topScreen = _screensStack.Peek();
			var embeddedRoot = topScreen.GetEmbeddedTransformRoot();
			var screen = _screenFactoryFacade.Create<TScreen, TContext>(context, embeddedRoot, _layer);
			topScreen.EmbeddedScreensStack.Push(screen);
			return screen;
		}
	}
}