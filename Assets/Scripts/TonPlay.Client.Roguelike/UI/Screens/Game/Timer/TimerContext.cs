using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Game.Timer.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.Timer
{
	public class TimerContext : ScreenContext, ITimerContext
	{
		public IReadOnlyReactiveProperty<double> TimeInSeconds { get; }

		public TimerContext(IReadOnlyReactiveProperty<double> timeInSeconds)
		{
			TimeInSeconds = timeInSeconds;
		}
	}
}