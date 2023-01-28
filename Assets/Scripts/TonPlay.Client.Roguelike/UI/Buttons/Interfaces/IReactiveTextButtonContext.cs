using UniRx;

namespace TonPlay.Client.Roguelike.UI.Buttons.Interfaces
{
	public interface IReactiveTextButtonContext : IButtonContext
	{
		IReadOnlyReactiveProperty<string> Text { get; }
	}
}