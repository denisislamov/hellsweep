namespace TonPlay.Roguelike.Client.UI.UIService.Interfaces
{
	public interface IScreenStack
	{
		IScreen Peek();

		void Push(IScreen screen);

		IScreen Pop();
	}
}