using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Pause.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Pause
{
	public class PauseScreenContext : ScreenContext, IPauseScreenContext
	{
		public System.Action ScreenClosedCallback { get; }
		
		public PauseScreenContext(System.Action screenClosedCallback)
		{
			ScreenClosedCallback = screenClosedCallback;
		}
	}
}