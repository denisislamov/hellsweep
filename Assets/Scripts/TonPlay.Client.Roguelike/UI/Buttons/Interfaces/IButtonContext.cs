using TonPlay.Client.Common.UIService.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Client.Roguelike.UI.Buttons.Interfaces
{
	public interface IButtonContext : IScreenContext
	{
		void Accept(IButtonContextVisitor buttonContextVisitor);
	}
}