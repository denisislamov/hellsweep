using System.Collections.Generic;

namespace TonPlay.Roguelike.Client.UI.UIService.Interfaces
{
	public interface IScreenStack : IEnumerable<IScreen>
	{
		IScreen Peek();

		void Push(IScreen screen);

		IScreen Pop();

		void Remove(IScreen screen);
	}
}