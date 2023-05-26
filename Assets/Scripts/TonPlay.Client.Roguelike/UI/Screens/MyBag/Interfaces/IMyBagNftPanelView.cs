using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Screens.MyBag.Interfaces
{
	public interface IMyBagNftPanelView : IView
	{
		IButtonView CloseButtonView { get; }
	}
}