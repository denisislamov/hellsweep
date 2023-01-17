using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;
using Zenject;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public class ScreenFactoryFacade : IScreenFactoryFacade
	{
		private readonly DiContainer _container;

		public ScreenFactoryFacade(DiContainer container)
		{
			_container = container;
		}
		
		public TScreen Create<TScreen, TContext>(TContext context, Transform root, IScreenLayer layer) 
			where TScreen : IScreen 
			where TContext : IScreenContext
		{
			var factory = _container.Resolve<IScreenFactory<TContext, TScreen>>();
			return factory.Create(context, root, layer);
		}
	}
}