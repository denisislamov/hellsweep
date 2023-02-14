using TonPlay.Client.Common.UIService;
using TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar.Interfaces;
using UniRx;

namespace TonPlay.Client.Roguelike.UI.Screens.Game.LevelProgressBar
{
	public class LevelProgressBarContext : ScreenContext, ILevelProgressBarContext
	{
		public IReadOnlyReactiveProperty<int> Level { get; }
		public IReadOnlyReactiveProperty<float> Value { get; }
		public IReadOnlyReactiveProperty<float> MaxValue { get; }

		public LevelProgressBarContext(
			IReadOnlyReactiveProperty<int> level,
			IReadOnlyReactiveProperty<float> value, 
			IReadOnlyReactiveProperty<float> maxValue)
		{
			Level = level;
			Value = value;
			MaxValue = maxValue;
		}
	}
}