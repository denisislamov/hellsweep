using System.Linq;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UnityEngine;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public class EmbeddedClosingScreenStrategy : IClosingScreenStrategy
	{
		private readonly IScreenStack _screensStack;
		
		public EmbeddedClosingScreenStrategy(IScreenStack screensStack)
		{
			_screensStack = screensStack;
		}
		
		public void Close(IScreen screen)
		{
			var peekScreen = _screensStack.Peek();
			if (peekScreen?.EmbeddedScreensStack.FirstOrDefault(_ => _ == screen) != null)
			{
				foreach (var embeddedScreen in screen.EmbeddedScreensStack)
				{
					embeddedScreen.Close();
					embeddedScreen.Dispose();
				}
				
				screen.Close();
				screen.Dispose();
				
				peekScreen.EmbeddedScreensStack.Remove(screen);

				Object.Destroy(screen.GameObject);
			}
		}
	}
}