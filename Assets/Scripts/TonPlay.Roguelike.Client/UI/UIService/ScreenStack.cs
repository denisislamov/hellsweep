using System.Collections.Generic;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public class ScreenStack : IScreenStack
	{
		private readonly Stack<IScreen> _screens = new Stack<IScreen>();

		public IScreen Peek() => _screens.Peek();

		public void Push(IScreen screen) => _screens.Push(screen);

		public IScreen Pop() => _screens.Pop();
	}
}