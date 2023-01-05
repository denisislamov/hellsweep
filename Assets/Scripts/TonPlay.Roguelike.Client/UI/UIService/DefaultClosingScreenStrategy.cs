using System;
using System.Linq;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using Object = UnityEngine.Object;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public class DefaultClosingScreenStrategy : IClosingScreenStrategy
	{
		private readonly IScreenStack _screenStack;
		
		public DefaultClosingScreenStrategy(IScreenStack screenStack)
		{
			_screenStack = screenStack;
		}

		public void Close(IScreen screen)
		{
			if (_screenStack.Any(_ => _.Equals(screen)))
			{
				foreach (var embeddedScreen in screen.EmbeddedScreensStack)
				{
					embeddedScreen.Close();
					embeddedScreen.Dispose();
				}
				
				screen.Close();
				screen.Dispose();
				
				_screenStack.Remove(screen);
				
				Object.Destroy(screen.GameObject);
			}
		}
	}
}