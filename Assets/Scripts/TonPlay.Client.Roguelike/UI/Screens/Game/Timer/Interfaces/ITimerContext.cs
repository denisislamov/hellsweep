using TonPlay.Roguelike.Client.UI.UIService.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.Timer.Interfaces
{
	public interface ITimerContext : IScreenContext
	{
		IReadOnlyReactiveProperty<float> TimeInSeconds { get; }
	}
}