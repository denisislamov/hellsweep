using UniRx;

namespace TonPlay.Client.Roguelike.UI.Buttons.Interfaces
{
	public interface IReactiveLockButtonContext : IButtonContext
	{
		IReadOnlyReactiveProperty<bool> Locked { get; }
	}
}