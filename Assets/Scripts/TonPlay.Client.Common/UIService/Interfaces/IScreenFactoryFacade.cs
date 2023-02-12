using TonPlay.Client.Common.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.UI.UIService.Interfaces
{
	public interface IScreenFactoryFacade
	{
		TScreen Create<TScreen, TContext>(TContext context, Transform root, IScreenLayer layer)
			where TContext : IScreenContext
			where TScreen : IScreen;
	}
}