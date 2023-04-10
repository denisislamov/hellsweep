using TonPlay.Client.Roguelike.UI.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Buttons.Interfaces
{
	public interface IButtonView : IView, IClickableView
	{
		void SetText(string text);

		void SetLockState(bool locked);
	}
}