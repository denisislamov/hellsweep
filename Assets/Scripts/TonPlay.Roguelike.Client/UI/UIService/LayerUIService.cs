using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public class LayerUIService : IUIService
	{
		private readonly Transform _rootTransform;

		private readonly IOpeningScreenStrategy _defaultOpeningStrategy;
		private readonly IOpeningScreenStrategy _embeddedOpeningStrategy;
		private readonly DefaultClosingScreenStrategy _defaultClosingStrategy;
		private readonly EmbeddedClosingScreenStrategy _embeddedClosingStrategy;

		public LayerUIService(IScreenFactoryFacade screenFactoryFacade, IScreenLayer layer, Transform rootTransform)
		{
			_rootTransform = rootTransform;
			
			IScreenStack screenStack = new ScreenStack();
			
			_defaultOpeningStrategy = new DefaultOpeningScreenStrategy(screenFactoryFacade, layer, rootTransform, screenStack);
			_embeddedOpeningStrategy = new EmbeddedOpeningScreenStrategy(screenFactoryFacade, layer, screenStack);
			_defaultClosingStrategy = new DefaultClosingScreenStrategy(screenStack);
			_embeddedClosingStrategy = new EmbeddedClosingScreenStrategy(screenStack);
		}
		
		public void Open<TScreen, TContext>(TContext context, bool isEmbedded = false, IScreenLayer screenLayer = null) 
			where TContext : IScreenContext
			where TScreen : IScreen
		{
			var openingScreenStrategy = GetOpeningScreenStrategy(isEmbedded);
			openingScreenStrategy.Open<TScreen, TContext>(context);
		}
		
		public void Close(IScreen screen, bool isEmbedded = false)
		{
			var closingScreenStrategy = GetClosingScreenStrategy(isEmbedded);
			closingScreenStrategy.Close(screen);
		}
		
		public Transform GetScreensRoot(IScreenLayer layer = null)
		{
			return _rootTransform;
		}

		private IOpeningScreenStrategy GetOpeningScreenStrategy(bool isEmbedded)
		{
			return isEmbedded ? _embeddedOpeningStrategy : _defaultOpeningStrategy;
		}
		
		private IClosingScreenStrategy GetClosingScreenStrategy(bool isEmbedded)
		{
			return isEmbedded ? _embeddedClosingStrategy : _defaultClosingStrategy;
		}
	}
}