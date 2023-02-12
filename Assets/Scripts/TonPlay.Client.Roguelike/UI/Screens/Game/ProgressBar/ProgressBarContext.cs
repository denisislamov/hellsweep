using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar.Interfaces;
using TonPlay.Roguelike.Client.UI.UIService;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.ProgressBar
{
	public class ProgressBarContext : ScreenContext, IProgressBarContext
	{
		public IReadOnlyReactiveProperty<float> MaxValue { get; }
		
		public IReadOnlyReactiveProperty<float> CurrentValue { get; }
		
		public ProgressBarContext(IReadOnlyReactiveProperty<float> currentValue, IReadOnlyReactiveProperty<float> maxValue)
		{
			CurrentValue = currentValue;
			MaxValue = maxValue;
		}
	}
}