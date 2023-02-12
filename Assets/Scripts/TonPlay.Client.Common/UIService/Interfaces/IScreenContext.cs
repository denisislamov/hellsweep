using TonPlay.Roguelike.Client.UI.UIService;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Common.UIService.Interfaces
{
	public interface IScreenContext
	{
		IScreen Screen { get; set; }
	}
}