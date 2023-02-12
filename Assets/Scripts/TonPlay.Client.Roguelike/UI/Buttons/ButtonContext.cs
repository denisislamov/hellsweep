using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Buttons.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;

namespace TonPlay.Client.Roguelike.UI.Buttons
{
	public abstract class ButtonContext : ScreenContext, IButtonContext
	{
		public abstract void Accept(IButtonContextVisitor buttonContextVisitor);
	}
}