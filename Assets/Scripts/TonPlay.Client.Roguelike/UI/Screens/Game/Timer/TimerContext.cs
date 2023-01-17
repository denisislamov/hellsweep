using TonPlay.Client.Roguelike.UI.Screens.Game.Timer.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.Timer
{
	public class TimerContext : ScreenContext, ITimerContext
	{
		public IReadOnlyReactiveProperty<float> TimeInSeconds { get; }

		public TimerContext(IReadOnlyReactiveProperty<float> timeInSeconds)
		{
			TimeInSeconds = timeInSeconds;
		}
	}
}