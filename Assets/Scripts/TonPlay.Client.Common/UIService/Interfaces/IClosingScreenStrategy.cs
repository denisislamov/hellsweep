using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Common.UIService.Interfaces
{
	public interface IClosingScreenStrategy
	{
		void Close(IScreen screen);
	}
}