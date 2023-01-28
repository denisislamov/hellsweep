using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Client.Common.UIService.Interfaces
{
	public interface IUIService
	{
		void Open<TScreen, TContext>(TContext context, bool isEmbedded = false, IScreenLayer screenLayer = null) 
			where TContext : IScreenContext
			where TScreen : IScreen;
		
		void Close(IScreen screen, bool isEmbedded = false);

		void CloseAll(IScreenLayer layer = null);
		
		Transform GetScreensRoot(IScreenLayer layer = null);
	}
}