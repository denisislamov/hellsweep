using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public class LayerUIService : IUIService
	{
		private readonly IScreenLayer _layer;
		private readonly Transform _rootTransform;

		private readonly IScreenStack _screenStack;

		private readonly IOpeningScreenStrategy _defaultOpeningStrategy;
		private readonly IOpeningScreenStrategy _embeddedOpeningStrategy;

		public LayerUIService(IScreenFactoryFacade screenFactoryFacade, IScreenLayer layer, Transform rootTransform)
		{
			_layer = layer;
			_rootTransform = rootTransform;

			_screenStack = new ScreenStack();
			
			_defaultOpeningStrategy = new DefaultOpeningScreenStrategy(screenFactoryFacade, layer, rootTransform, _screenStack);
			_embeddedOpeningStrategy = new EmbeddedOpeningScreenStrategy(screenFactoryFacade, layer, _screenStack);
		}
		
		public void Open<TScreen, TContext>(TContext context, bool isEmbedded = false, IScreenLayer screenLayer = null) 
			where TContext : IScreenContext
			where TScreen : IScreen
		{
			var openingScreenStrategy = GetOpeningScreenStrategy(isEmbedded);
			openingScreenStrategy.Open<TScreen, TContext>(context);
		}
		
		public void Close(IScreen screen)
		{
			
		}

		private IOpeningScreenStrategy GetOpeningScreenStrategy(bool isEmbedded)
		{
			return isEmbedded ? _embeddedOpeningStrategy : _defaultOpeningStrategy;
		}
	}
}