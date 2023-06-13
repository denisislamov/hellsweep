using TonPlay.Client.Common.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.Pause.Interfaces
{
	public interface IPauseScreenContext : IScreenContext
	{
		System.Action ScreenClosedCallback { get; }
	}
}