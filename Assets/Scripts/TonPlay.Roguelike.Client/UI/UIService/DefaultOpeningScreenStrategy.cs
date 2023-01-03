using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public class DefaultOpeningScreenStrategy : OpeningScreenStrategy
	{
		private readonly IScreenFactoryFacade _screenFactoryFacade;
		private readonly IScreenStack _screenStack;
		private readonly Transform _rootTransform;
		private readonly IScreenLayer _layer;

		public DefaultOpeningScreenStrategy(IScreenFactoryFacade screenFactoryFacade, IScreenLayer layer, Transform rootTransform, IScreenStack screenStack)
		{
			_screenFactoryFacade = screenFactoryFacade;
			_rootTransform = rootTransform;
			_screenStack = screenStack;
			_layer = layer;
		}
		
		protected override TScreen Create<TScreen, TContext>(TContext context)
		{
			var screen = _screenFactoryFacade.Create<TScreen, TContext>(context, _rootTransform, _layer);
			_screenStack.Push(screen);
			return screen;
		}
	}
}