using System.Collections;
using System.Collections.Generic;
using TonPlay.Roguelike.Client.UI.UIService.Interfaces;

namespace TonPlay.Roguelike.Client.UI.UIService
{
	public class ScreenStack : IScreenStack
	{
		private readonly List<IScreen> _screens = new List<IScreen>();

		public IScreen Peek() =>
			_screens.Count == 0 
				? default 
				: _screens[_screens.Count - 1];

		public void Push(IScreen screen) => _screens.Add(screen);

		public IScreen Pop()
		{
			if (_screens.Count == 0)
			{
				return default;
			}
			
			var screen = Peek();
			_screens.RemoveAt(_screens.Count - 1);
			return screen;
		}
		
		public void Remove(IScreen screen) => _screens.Remove(screen);

		public IEnumerator<IScreen> GetEnumerator() => _screens.GetEnumerator();
		
		IEnumerator IEnumerable.GetEnumerator() => _screens.GetEnumerator();
	}
}