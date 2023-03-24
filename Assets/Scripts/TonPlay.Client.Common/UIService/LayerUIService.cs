using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Common.UIService
{
	public class LayerUIService : IUIService
	{
		private readonly Transform _rootTransform;

		private readonly IOpeningScreenStrategy _defaultOpeningStrategy;
		private readonly IOpeningScreenStrategy _embeddedOpeningStrategy;
		private readonly IClosingScreenStrategy _defaultClosingStrategy;
		private readonly IClosingScreenStrategy _embeddedClosingStrategy;
		
		private readonly ScreenStack _screenStack;

		public LayerUIService(IScreenFactoryFacade screenFactoryFacade, IScreenLayer layer, Transform rootTransform)
		{
			_rootTransform = rootTransform;
			
			_screenStack = new ScreenStack();
			
			_defaultOpeningStrategy = new DefaultOpeningScreenStrategy(screenFactoryFacade, layer, rootTransform, _screenStack);
			_embeddedOpeningStrategy = new EmbeddedOpeningScreenStrategy(screenFactoryFacade, layer, _screenStack);
			_defaultClosingStrategy = new DefaultClosingScreenStrategy(_screenStack);
			_embeddedClosingStrategy = new EmbeddedClosingScreenStrategy(_screenStack);
		}
		
		public TScreen Open<TScreen, TContext>(TContext context, bool isEmbedded = false, IScreenLayer screenLayer = null) 
			where TContext : IScreenContext
			where TScreen : IScreen
		{
			var openingScreenStrategy = GetOpeningScreenStrategy(isEmbedded);
			return openingScreenStrategy.Open<TScreen, TContext>(context);
		}
		
		public void Close(IScreen screen, bool isEmbedded = false)
		{
			var closingScreenStrategy = GetClosingScreenStrategy(isEmbedded);
			closingScreenStrategy.Close(screen);
		}
		
		public void CloseAll(IScreenLayer layer = null)
		{
			while(_screenStack.Peek() != null)
			{
				Close(_screenStack.Peek());
			}
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
		
		public void Dispose()
		{
			if (_screenStack == null) return;
			
			foreach (var screen in _screenStack)
			{
				screen.Dispose();
			}
		}
	}
}